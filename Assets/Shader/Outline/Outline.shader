Shader "Team3/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Space(20)]
        [Header(Outline)]
        [HDR]_OutLineColor ("Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Width", Range(0.0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        ZWrite Off
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ZTest Off
            
            Stencil
            {
                Ref 1
                Comp Greater
                Pass Replace  // ステンシルに1を書き込む
                ZFail Zero    // 深度テストが失敗したらステンシル値を 0 にする
            }
            

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _OutLineColor;
            float _OutlineWidth;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings input) : SV_Target
            {
                half4 col = _OutLineColor;
                half2 uv = input.uv;
                
                half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x + _OutlineWidth, uv.y)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, uv.y + _OutlineWidth)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x - _OutlineWidth, uv.y)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, uv.y - _OutlineWidth)).a;

                alpha = saturate(alpha);

                col.a *= alpha;
                col.rgb *= col.a;

                clip(alpha - 0.001);
                return col;
            }
            ENDHLSL
        }
        
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _OutLineColor;
            float _OutlineWidth;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings input) : SV_Target
            {
                half4 col;
                half2 uv = input.uv;

                col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                
                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _OutLineColor;
            float _OutlineWidth;
            CBUFFER_END

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
            };

            Varyings ShadowPassVertex(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
                return OUT;
            }

            void ShadowPassFragment(Varyings IN)
            {
                float alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;

                clip(alpha - 0.001);
            }
            ENDHLSL
        }
        // Depth を書き込むためのパス
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }

            ZWrite On
            ColorMask R
            Cull[_Cull]
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _OutLineColor;
            float _OutlineWidth;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half frag(Varyings IN) : SV_Target
            {
                float alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;

                clip(alpha - 0.001);
                
                return  IN.positionHCS.z;
            }
            ENDHLSL
        }
        
        
        // This pass is used when drawing to a _CameraNormalsTexture texture
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }

            ZWrite On
            Cull Back
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            half4 _OutLineColor;
            float _OutlineWidth;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;

                clip(alpha - 0.001);
                
                // 法線を 0-1 範囲にリマップして格納
                return half4(IN.normalWS, 0.0);
            }
            ENDHLSL
        }
    }
}
