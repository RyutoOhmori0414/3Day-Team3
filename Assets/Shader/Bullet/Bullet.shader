Shader "Team3/Bullet"
{
    Properties
    {
        [HDR] _Color ("Color", Color) = (1, 1, 1, 1)
        [HDR] _RimColor ("RimColor", Color) = (1, 1, 0, 1)
        _RimPower("RimPower", Range(0.5, 5)) = 1
        [Space(20)]
        [Header(Lightning)]
        _LightningTex ("Tex", 2D) = "black" {}
        [HDR]_LightningColor ("Color", Color) = (1, 1, 1, 1)
        
        [Space(20)]
        [Header(Outline)]
        _OutlineWidth ("Width", Range(0.0, 3.0)) = 0.5
        [HDR] _OutlineColor ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Front
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_LightningTex); SAMPLER(sampler_LightningTex);

            CBUFFER_START(UnityPerMaterial)

            half4 _Color;
            half4 _RimColor;
            float _RimPower;
            float4 _LightningTex_ST;
            half4 _LightningColor;
            
            // Outline
            float _OutlineWidth;
            half4 _OutlineColor;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                positionWS = normalWS * _OutlineWidth + positionWS;

                output.positionCS = TransformWorldToHClip(positionWS);

                return output;
            }

            half4 frag (Varyings i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
        
        Pass
        {
            Tags {"LightMode" = "UniversalForward"}
            Cull back
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            TEXTURE2D(_LightningTex); SAMPLER(sampler_LightningTex);
            
            CBUFFER_START(UnityPerMaterial)

            half4 _RimColor;
            half4 _Color;
            float _RimPower;

            float4 _LightningTex_ST;
            half4 _LightningColor;
            
            // Outline
            float _OutlineWidth;
            half4 _OutlineColor;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.normalWS = normalize(TransformObjectToWorldNormal(input.normalOS.xyz));
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.viewDirWS = normalize(GetWorldSpaceViewDir(output.positionWS));

                output.uv = TRANSFORM_TEX(input.texcoord.xy, _LightningTex);

                return output;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float rim = 1.0 - saturate(dot(i.normalWS, i.viewDirWS));
                rim = pow(rim, _RimPower);

                float righting = SAMPLE_TEXTURE2D(_LightningTex, sampler_LightningTex, i.uv + _Time.y).r;
                
                half4 col = lerp(_Color, _RimColor, rim);
                col = lerp(col, _LightningColor, righting);
                
                return col;
            }
            ENDHLSL
        }
    }
}
