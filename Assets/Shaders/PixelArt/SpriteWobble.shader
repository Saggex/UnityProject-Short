Shader "Custom/PixelArt/Wobble"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Amplitude ("Wobble Amplitude", Range(0,0.5)) = 0.05
        _Frequency ("Wobble Frequency", Float) = 10
        _Speed ("Wobble Speed", Float) = 5
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
            float _Amplitude;
            float _Frequency;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y += sin(uv.x * _Frequency + _Time.y * _Speed) * _Amplitude;
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
