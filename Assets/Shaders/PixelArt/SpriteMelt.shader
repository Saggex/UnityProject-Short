Shader "Custom/PixelArt/Melt"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
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
            float _Amount;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            float2 ApplyMelt(float2 uv)
            {
                float melt = frac(_Time.y * _Speed) * _Amount;
                uv.y += melt * (1 - uv.y);
                uv.x += sin(uv.y * 40 + _Time.y * 10) * 0.05 * melt;
                return uv;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = ApplyMelt(i.uv);
                return tex2D(_MainTex, uv) * i.color;
            }
            ENDCG
        }
    }
}
