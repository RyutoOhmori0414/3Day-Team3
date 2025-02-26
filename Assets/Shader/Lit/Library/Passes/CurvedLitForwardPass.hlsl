#ifndef TEAM3_CURVED_LIT_FORWARD_PASS_INCLUDED
#define TEAM3_CURVED_LIT_FORWARD_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "../Library/Curved/Curved.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float2 uv : TEXCOORD0;
    float2 lightmapUv : TEXCOORD1;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;
    float4 positionHCS : SV_POSITION;
    float3 normalWS : NORMAL;
    float3 tangentWS : TANGENT;
    float3 bitangentWS : TEXCOORD1;
    float3 positionWS : TEXCOORD2;
    float4 shadowCoord : TEXCOORD3;
    half fogFactor : TEXCOORD5;
    float3 vertexLight : TEXCOORD6;
                

    DECLARE_LIGHTMAP_OR_SH(lightmapUv, vertexSH, 7);
                
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};


void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData = (InputData)0;

    inputData.positionWS = input.positionWS;
    inputData.positionCS = input.positionHCS;

    inputData.fogCoord = input.fogFactor;
    inputData.shadowCoord = input.shadowCoord;
    inputData.normalWS = input.normalWS;
}

void InitializeBakedGIData(Varyings input, inout InputData inputData)
{
    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);
}

Varyings LitPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                VertexPositionInputs curvedVertexInput;
                CalcVertexCurve(_CurveFactor, _CurveOffset, _CurveStrength, _CurveHeightOffset,
                    GetVertexPositionInputs(input.positionOS.xyz), curvedVertexInput);
                
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                float3 viewDirWS = GetWorldSpaceViewDir(curvedVertexInput.positionWS);
                output.vertexLight = VertexLighting(curvedVertexInput.positionWS, normalInput.normalWS);
                output.fogFactor = ComputeFogFactor(curvedVertexInput.positionCS.z);

                output.positionHCS = curvedVertexInput.positionCS;
                output.positionWS = curvedVertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.tangentWS = normalInput.tangentWS;
                output.bitangentWS = normalInput.bitangentWS;

                OUTPUT_LIGHTMAP_UV(input.lightmapUv, unity_LightmapST, output.lightmapUv);
                OUTPUT_SH(output.normalWS, output.vertexSH);

                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.shadowCoord = GetShadowCoord(curvedVertexInput);

                return output;
}

void LitPassFragment(Varyings input, out half4 outColor : SV_Target0)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    SurfaceData surfaceData = (SurfaceData)0;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    InputData inputData = (InputData)0;
    InitializeInputData(input, surfaceData.normalTS, inputData);

    InitializeBakedGIData(input, inputData);

    half4 color = UniversalFragmentPBR(inputData, surfaceData);
    color.rgb = MixFog(color.rgb, inputData.fogCoord);

    outColor = color;
}

#endif