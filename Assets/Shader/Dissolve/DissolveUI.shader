Shader "Team3/Dissolve"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Space(20)]
        [Header(Dissolve)]
        _DissolveTex("Tex", 2D) = "white" {}
        _DissolveAmount("Amount", Range(0.0, 1.0)) = 0.5
        _DissolveRange("Range", Range(0.0, 1.0)) = 0.1
        [HDR]_DissolveColor("Color", Color) = (1, 0, 0, 1)
        

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
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
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
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half4 color    : COLOR;
                float2 uv  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                float2 dissolveUV : TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_DissolveTex); SAMPLER(sampler_DissolveTex);

            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            half4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            // Dissolve
            float _DissolveAmount;
            float _DissolveRange;
            half4 _DissolveColor;
            float4 _DissolveTex_ST;
            CBUFFER_END

            #define REMAP(value, inMin, inMax, outMin, outMax) (value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                float4 vPosition = TransformObjectToHClip(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;
                
		
                float2 pixelSize = vPosition.w;
		
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
		
                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
                OUT.dissolveUV = TRANSFORM_TEX(v.texcoord.xy, _DissolveTex);
                
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                half4 color = IN.color * (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) + _TextureSampleAdd);

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                float dissolveAlpha = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, IN.dissolveUV).r;
                dissolveAlpha += 0.001;

                float amount = REMAP(_DissolveAmount, 0, 1, -_DissolveRange, 1);

                color += lerp(_DissolveColor, 0, step(amount + _DissolveRange, dissolveAlpha));

                color.a = lerp(0, color.a, step(amount, dissolveAlpha)); 
                
                color.rgb *= color.a;

                return color;
            }
            ENDHLSL
        }
    }
}