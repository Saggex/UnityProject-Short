Shader "Custom/PixelArt/Morph"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _NoiseScale ("Noise Scale", Float) = 4
        _Speed ("Morph Speed", Float) = 1
        _Intensity ("Morph Intensity", Range(0,1)) = 0.1
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
            fixed4 _Color;
            float _NoiseScale;
            float _Speed;
            float _Intensity;

            float hash12(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float perlin(float2 uv)
            {
                float2 id = floor(uv);
                float2 gv = frac(uv);
                float a = hash12(id);
                float b = hash12(id + float2(1,0));
                float c = hash12(id + float2(0,1));
                float d = hash12(id + float2(1,1));
                float2 u = gv * gv * (3.0 - 2.0 * gv);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            float2 curl(float2 uv)
            {
                float eps = 0.001;
                float n1 = perlin(uv + float2(0, eps));
                float n2 = perlin(uv - float2(0, eps));
                float a = (n1 - n2) / (2.0 * eps);
                n1 = perlin(uv + float2(eps, 0));
                n2 = perlin(uv - float2(eps, 0));
                float b = (n1 - n2) / (2.0 * eps);
                return float2(a, -b);
            }

            float2 ApplyMorph(float2 uv)
            {
                float2 c = curl(uv * _NoiseScale + _Time.y * _Speed);
                return uv + c * _Intensity;
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
                float2 uv = ApplyMorph(i.uv);
                return tex2D(_MainTex, uv) * i.color;
            }
            ENDCG
        }
    }
}
