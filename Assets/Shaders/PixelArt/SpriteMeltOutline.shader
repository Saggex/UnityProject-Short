Shader "Custom/PixelArt/MeltOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineSize ("Outline Size", Float) = 1
        _Speed ("Melt Speed", Float) = 2
        _BlockSize ("Block Size", Float) = 50
        _Intensity ("Melt Intensity", Range(0,1)) = 1
        _Jitter ("Horizontal Jitter", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineSize;
            float _Speed;
            float _BlockSize;
            float _Intensity;
            float _Jitter;

            float rand(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float2 ApplyMelt(float2 uv)
            {
                float t = _Time.y * _Speed;
                float2 ruv = uv * _BlockSize;
                float2 id = floor(ruv);
                float rnd = rand(id.xx);
                float2 offsetUv = ruv;
                offsetUv.y += t * (rnd * 0.75 + 0.5) * _Intensity;
                float2 guv = frac(offsetUv);
                offsetUv = floor(offsetUv) + guv;
                uv = offsetUv / _BlockSize;
                uv.x += sin(uv.y * 10 + t) * _Jitter * _Intensity;
                return uv;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = ApplyMelt(i.uv);
                fixed4 c = tex2D(_MainTex, uv) * i.color;
                if (c.a == 0)
                {
                    float2 offset = _MainTex_TexelSize.xy * _OutlineSize;
                    float alpha = 0;
                    alpha += tex2D(_MainTex, ApplyMelt(i.uv + float2(offset.x, 0))).a;
                    alpha += tex2D(_MainTex, ApplyMelt(i.uv + float2(-offset.x, 0))).a;
                    alpha += tex2D(_MainTex, ApplyMelt(i.uv + float2(0, offset.y))).a;
                    alpha += tex2D(_MainTex, ApplyMelt(i.uv + float2(0, -offset.y))).a;
                    if (alpha > 0)
                    {
                        return _OutlineColor * i.color;
                    }
                    return 0;
                }
                return c;
            }
            ENDCG
        }
    }
}
