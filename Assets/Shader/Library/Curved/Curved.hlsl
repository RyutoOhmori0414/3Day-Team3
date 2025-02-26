#ifndef TEAM3_CURVED_INCLUDED
#define TEAM3_CURVED_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#define VERTEX_CURVE_UNIFORM\
float _CurveFactor;\
float _CurveOffset;\
float _CurveStrength;\
float _CurveHeightOffset;

inline void CalcVertexCurve(float factor, float offset, float strength, float heightOffset, VertexPositionInputs vertPosInput, out VertexPositionInputs curvedVertexInput)
{
    float3 curvedPosWS = vertPosInput.positionWS;
    float distance = pow(length(vertPosInput.positionWS.z - _WorldSpaceCameraPos.z + offset), factor);

    curvedPosWS.y -= distance * strength;
    curvedPosWS.y += heightOffset;

    curvedVertexInput = GetVertexPositionInputs(TransformWorldToObject(curvedPosWS).xyz);
}

#endif