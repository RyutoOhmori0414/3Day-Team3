#ifndef TEAM3_CURVED_LIT_INPUT_INCLUDED
#define TEAM3_CURVED_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "../Library/Curved/Curved.hlsl"


CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;

half _Cutoff;
half4 _BaseColor;
half4 _SpecColor;
half4 _EmissionColor;
half _Smoothness;
half _Metallic;
float _CurveFactor;
float _CurveOffset;
float _CurveStrength;
float _CurveHeightOffset;
CBUFFER_END

inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = (SurfaceData)0;
    
    half4 albedoAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    outSurfaceData.alpha = albedoAlpha.a * _BaseColor.a;

    outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;

    outSurfaceData.specular = _SpecColor.rgb;
    outSurfaceData.emission = _EmissionColor.rgb;
    outSurfaceData.smoothness = _Smoothness;
    outSurfaceData.metallic = _Metallic;
}

#endif