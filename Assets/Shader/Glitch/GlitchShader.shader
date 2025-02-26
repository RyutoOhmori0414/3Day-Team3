Shader "Team3/Glitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        
        _GlitchStrength ("Glitch Strength", Range(0, 1)) = 1
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.1
        _BlockScale("Block Scale", Range(1,50)) = 10
        _NoiseSpeed("Noise Speed", Range(1,10)) = 10
        _SlideScale("Slide", Range(0.0, 0.3)) = 0.1

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType"="Plane"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            float4 _MainTex_ST;
            half4 _TextureSampleAdd;

            float _GlitchStrength;
            float _GlitchIntensity;
            float _BlockScale;
            float _NoiseSpeed;
            float _SlideScale;
            CBUFFER_END

            inline float random(float2 seeds)
            {
                return frac(sin(dot(seeds, float2(12.9898, 78.233))) * 43758.5453);
            }

            inline float blockNose(float2 seeds)
            {
                return random(floor(seeds));
            }

            inline float noiseRandom(float2 seeds)
            {
                return -1.0 + 2.0 * blockNose(seeds);
            }

            float perlinNoise(half2 st)
            {
                half2 p = floor(st);
                half2 f = frac(st);
                half2 u = f * f * (3.0 - 2.0 * f);

                float v00 = random(p + half2(0, 0));
                float v10 = random(p + half2(1, 0));
                float v01 = random(p + half2(0, 1));
                float v11 = random(p + half2(1, 1));

                return lerp(lerp(dot(v00, f - half2(0, 0)), dot(v10, f - half2(1, 0)), u.x),
                            lerp(dot(v01, f - half2(0, 1)), dot(v11, f - half2(1, 1)), u.x),
                            u.y) + 0.5f;
            }

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                
                float2 gv = input.uv;
                float noise = blockNose(input.uv.y * _BlockScale);
                noise += random(input.uv.x) * 0.3;
                float2 randomValue = noiseRandom(float2(input.uv.y, _Time.x * _NoiseSpeed));
                gv.x = lerp(gv.x, gv.x + randomValue * sin(sin(_GlitchIntensity) * 0.5) * sin(-sin(noise) * 0.2) * frac(_Time.y), _GlitchStrength);
                half4 r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, gv + float2(frac(perlinNoise(-_Time.y * _NoiseSpeed)) * _SlideScale, 0)) + _TextureSampleAdd;
                half4 g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, gv)  + _TextureSampleAdd;
                half4 b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, gv - float2(frac(perlinNoise(_Time.y * _NoiseSpeed)) * _SlideScale, 0))  + _TextureSampleAdd;

                r.rgb *= r.a;
                g.rgb *= g.a;
                b.rgb *= b.a;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv)  + _TextureSampleAdd;
                col.rgb *= col.a;
                col += half4(r.r, g.g, b.b, (r.a + g.a + b.a) / 3);
                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
    }
}
