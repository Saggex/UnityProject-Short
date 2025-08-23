Shader "Custom/SpriteOutline" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineSize ("Outline Size", Float) = 1
    }
    SubShader {
        Tags {"RenderType"="Transparent" "Queue"="Transparent"}
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        // First pass: draw outline
        Pass {
            Name "OUTLINE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineSize;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv);
                if (c.a == 0) {
                    float2 offsets[8] = {
                        float2(_OutlineSize * _MainTex_TexelSize.x, 0),
                        float2(-_OutlineSize * _MainTex_TexelSize.x, 0),
                        float2(0, _OutlineSize * _MainTex_TexelSize.y),
                        float2(0, -_OutlineSize * _MainTex_TexelSize.y),
                        float2(_OutlineSize * _MainTex_TexelSize.x, _OutlineSize * _MainTex_TexelSize.y),
                        float2(-_OutlineSize * _MainTex_TexelSize.x, _OutlineSize * _MainTex_TexelSize.y),
                        float2(_OutlineSize * _MainTex_TexelSize.x, -_OutlineSize * _MainTex_TexelSize.y),
                        float2(-_OutlineSize * _MainTex_TexelSize.x, -_OutlineSize * _MainTex_TexelSize.y)
                    };
                    for (int j = 0; j < 8; j++) {
                        if (tex2D(_MainTex, i.uv + offsets[j]).a > 0) {
                            return _OutlineColor;
                        }
                    }
                    return 0;
                }
                return 0;
            }
            ENDCG
        }

        // Second pass: draw sprite normally
        Pass {
            Name "SPRITE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;
                return c;
            }
            ENDCG
        }
    }
}
