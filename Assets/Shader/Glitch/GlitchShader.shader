Shader "Team3/Glitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.1
        _BlockScale("Block Scale", Range(1,50)) = 10
        _NoiseSpeed("Noise Speed", Range(1,10)) = 10

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
        Tags { "RenderType"="Opaque" }
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
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            float4 _MainTex_ST;
            half4 _TextureSampleAdd;

            float _GlitchIntensity;
            float _BlockScale;
            float _NoiseSpeed;
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

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                half4 col = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) + _TextureSampleAdd);

                float2 gv = input.uv;
                float noise = blockNose(input.uv.y * _BlockScale);
                noise += random(input.uv.x) * 0.3;
                float2 randomValue = noiseRandom(float2(input.uv.y, _Time.y * _NoiseSpeed));
                gv.x += randomValue * sin(sin(_GlitchIntensity) * 0.5) * sin(-sin(noise) * 0.2) * frac(_Time.y);
                col.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, gv + float2(0.0006, 0)).r;
                col.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, gv).g;
                col.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, gv - float2(0.008, 0)).b;
                col.a = 1.0;
                
                return col;
            }
            ENDHLSL
        }
    }
}
