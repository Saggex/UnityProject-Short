Shader "Custom/CreepySprite"
{
    Properties
    {
        [PerRendererData]_MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _NoiseTex("Noise", 2D) = "white" {}
        _GlitchIntensity("Glitch Intensity", Range(0,1)) = 0
        _GlitchColorOffset("Glitch Color Offset", Range(0,1)) = 0
        _AuraColor("Aura Color", Color) = (0,1,0,1)
        _AuraThickness("Aura Thickness", Range(0,0.1)) = 0
        _AuraPulseSpeed("Aura Pulse Speed", Range(0,10)) = 0
        _MeltAmount("Melt Amount", Range(0,1)) = 0
        _MeltSpeed("Melt Speed", Range(0,5)) = 0
        _CrawlAmount("Crawl Amount", Range(0,1)) = 0
        _CrawlSpeed("Crawl Speed", Range(0,5)) = 0
        _DissolveAmount("Dissolve Amount", Range(0,1)) = 0
        _DissolveEdgeColor("Dissolve Edge Color", Color) = (1,0,0,1)
        _FlickerIntensity("Flicker Intensity", Range(0,1)) = 0
        _FlickerSpeed("Flicker Speed", Range(0,50)) = 0
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
        LOD 100

        Pass
        {
            Name "CREEPY"
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex); SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _GlitchIntensity;
                float _GlitchColorOffset;
                float4 _AuraColor;
                float _AuraThickness;
                float _AuraPulseSpeed;
                float _MeltAmount;
                float _MeltSpeed;
                float _CrawlAmount;
                float _CrawlSpeed;
                float _DissolveAmount;
                float4 _DissolveEdgeColor;
                float _FlickerIntensity;
                float _FlickerSpeed;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD1;
                float4 color : COLOR;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float2 uv = IN.uv;

                // Glitch UV distortion
                float2 glitchNoise = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, uv * 2.0 + _Time.yz, 0).rg - 0.5;
                uv += glitchNoise * _GlitchIntensity;

                // Dripping / Melting
                float melt = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, float2(uv.x, uv.y + _Time.x * _MeltSpeed), 0).r;
                uv.y -= saturate(melt) * _MeltAmount;

                // Flesh-crawling UV shift
                float crawl = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, uv * 4.0 + _Time.yx * _CrawlSpeed, 0).r;
                uv += (crawl - 0.5) * _CrawlAmount * 0.02;

                OUT.uv = uv;
                OUT.noiseUV = IN.uv;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.color = _Color;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;

                // Ghostly Dissolve
                float dissolveNoise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, IN.noiseUV + _Time.xy).r;
                float edge = step(_DissolveAmount, dissolveNoise);
                float edgeGlow = smoothstep(_DissolveAmount - 0.05, _DissolveAmount, dissolveNoise);
                col.rgb = lerp(_DissolveEdgeColor.rgb, col.rgb, edge);
                col.a *= edge;

                // Unnatural Color Flicker
                float flicker = (sin(_Time.y * _FlickerSpeed) * 0.5 + 0.5) * _FlickerIntensity;
                col.rgb = lerp(col.rgb, 1 - col.rgb, flicker);

                // Glitch color separation
                float2 offset = float2(_GlitchColorOffset * 0.01, 0);
                float r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offset).r;
                float g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).g;
                float b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - offset).b;
                col.rgb = lerp(col.rgb, float3(r, g, b), _GlitchIntensity);

                // Pulsing shadow / aura outline
                float2 pixel = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                float pulse = _AuraThickness + sin(_Time.y * _AuraPulseSpeed) * _AuraThickness;
                float2 delta = pixel * pulse;
                float alphaLeft = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.noiseUV + float2(-delta.x, 0)).a;
                float alphaRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.noiseUV + float2(delta.x, 0)).a;
                float alphaUp = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.noiseUV + float2(0, delta.y)).a;
                float alphaDown = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.noiseUV + float2(0, -delta.y)).a;
                float outline = max(max(alphaLeft, alphaRight), max(alphaUp, alphaDown));
                float aura = saturate(outline - col.a);
                col.rgb = lerp(col.rgb, _AuraColor.rgb, aura * _AuraColor.a);
                col.a += aura * _AuraColor.a;

                return col;
            }
            ENDHLSL
        }
    }
}
