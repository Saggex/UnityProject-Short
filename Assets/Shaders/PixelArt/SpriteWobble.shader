Shader "Custom/PixelArt/Wobble"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Amplitude ("Wobble Amplitude", Range(0,0.5)) = 0.05
        _Frequency ("Wobble Frequency", Float) = 10
        _Speed ("Wobble Speed", Float) = 5
        _Direction ("Wobble Direction", Vector) = (1,0,0,0)
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
            float _Amplitude;
            float _Frequency;
            float _Speed;
            float4 _Direction;

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
                float2 uv = i.uv;
                float2 dir = normalize(_Direction.xy);
                float wave = sin(dot(uv, dir) * _Frequency + _Time.y * _Speed) * _Amplitude;
                float2 perp = float2(-dir.y, dir.x);
                uv += perp * wave;
                return tex2D(_MainTex, uv) * i.color;
            }
            ENDCG
        }
    }
}
