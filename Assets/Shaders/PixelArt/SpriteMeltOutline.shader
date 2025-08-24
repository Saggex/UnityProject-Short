Shader "Custom/PixelArt/MeltOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineSize ("Outline Size", Float) = 1
        _Amount ("Melt Amount", Range(0,1)) = 1
        _Speed ("Melt Speed", Float) = 1
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _OutlineColor;
            float _OutlineSize;
            float _Amount;
            float _Speed;

            float2 ApplyMelt(float2 uv)
            {
                float melt = frac(_Time.y * _Speed) * _Amount;
                uv.y += melt * (1 - uv.y);
                uv.x += sin(uv.y * 40 + _Time.y * 10) * 0.05 * melt;
                return uv;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = ApplyMelt(i.uv);
                fixed4 c = tex2D(_MainTex, uv);
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
                        return _OutlineColor;
                    }
                    return 0;
                }
                return c;
            }
            ENDCG
        }
    }
}
