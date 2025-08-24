Shader "Custom/PixelArt/MorphOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineSize ("Outline Size", Float) = 1
        _Amplitude ("Morph Amplitude", Range(0,1)) = 0.1
        _Frequency ("Morph Frequency", Float) = 5
        _Speed ("Morph Speed", Float) = 1
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
            float _Amplitude;
            float _Frequency;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                float wave = sin(_Time.y * _Speed + v.vertex.x * _Frequency) * _Amplitude;
                v.vertex.y += wave;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;
                if (c.a == 0)
                {
                    float2 offset = _MainTex_TexelSize.xy * _OutlineSize;
                    float alpha = 0;
                    alpha += tex2D(_MainTex, i.uv + float2(offset.x, 0)).a;
                    alpha += tex2D(_MainTex, i.uv + float2(-offset.x, 0)).a;
                    alpha += tex2D(_MainTex, i.uv + float2(0, offset.y)).a;
                    alpha += tex2D(_MainTex, i.uv + float2(0, -offset.y)).a;
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
