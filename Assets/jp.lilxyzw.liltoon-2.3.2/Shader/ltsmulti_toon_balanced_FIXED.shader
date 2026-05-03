Shader "Custom/ltsmulti_toon_balanced_FIXED"
{
    Properties
    {
        [MainColor] _Color ("Color", Color) = (1,1,1,1)
        [MainTexture] _MainTex ("Main Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        [Toggle(_ALPHATEST_ON)] _AlphaClip ("Alpha Clip", Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2

        [Header(Toon Lighting)]
        _AsUnlit ("As Unlit / Original Color", Range(0,1)) = 0.35
        _LightMinLimit ("Minimum Brightness", Range(0,1)) = 0.28
        _LightMaxLimit ("Maximum Brightness", Range(0,3)) = 1.15
        _LightStrength ("Main Light Strength", Range(0,2)) = 1.0
        _ShadowColor ("1st Shadow Color", Color) = (0.70,0.74,0.92,1)
        _Shadow2ndColor ("2nd Shadow Color", Color) = (0.40,0.45,0.68,1)
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.65
        _ShadowBorder ("1st Shadow Border", Range(0,1)) = 0.45
        _ShadowBlur ("1st Shadow Blur", Range(0.001,0.5)) = 0.08
        _Shadow2ndBorder ("2nd Shadow Border", Range(0,1)) = 0.20
        _Shadow2ndBlur ("2nd Shadow Blur", Range(0.001,0.5)) = 0.08
        _SaturationBoost ("Saturation Boost", Range(0,2)) = 1.15

        [Header(Rim)]
        _RimColor ("Rim Color", Color) = (0.55,0.62,0.90,1)
        _RimStrength ("Rim Strength", Range(0,1)) = 0.10
        _RimPower ("Rim Power", Range(0.5,8)) = 3.5
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }
            Cull [_Cull]
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _MainTex_ST;
                half _Cutoff;
                half _AlphaClip;
                half _Cull;
                half _AsUnlit;
                half _LightMinLimit;
                half _LightMaxLimit;
                half _LightStrength;
                half4 _ShadowColor;
                half4 _Shadow2ndColor;
                half _ShadowStrength;
                half _ShadowBorder;
                half _ShadowBlur;
                half _Shadow2ndBorder;
                half _Shadow2ndBlur;
                half _SaturationBoost;
                half4 _RimColor;
                half _RimStrength;
                half _RimPower;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                half3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                VertexPositionInputs pos = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs nrm = GetVertexNormalInputs(v.normalOS);
                o.positionCS = pos.positionCS;
                o.positionWS = pos.positionWS;
                o.normalWS = normalize(nrm.normalWS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half3 BoostSaturation(half3 c, half sat)
            {
                half g = dot(c, half3(0.299h, 0.587h, 0.114h));
                return lerp(half3(g, g, g), c, sat);
            }

            half4 frag(Varyings i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _Color;

                #if defined(_ALPHATEST_ON)
                    clip(tex.a - _Cutoff);
                #endif

                half3 N = normalize(i.normalWS);
                half3 V = normalize(GetWorldSpaceViewDir(i.positionWS));

                float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                Light mainLight = GetMainLight(shadowCoord);

                half ndl = saturate(dot(N, mainLight.direction));
                half lightValue = saturate(ndl * mainLight.shadowAttenuation * _LightStrength);

                half litMask = smoothstep(_ShadowBorder - _ShadowBlur, _ShadowBorder + _ShadowBlur, lightValue);
                half deepMask = 1.0h - smoothstep(_Shadow2ndBorder - _Shadow2ndBlur, _Shadow2ndBorder + _Shadow2ndBlur, lightValue);

                half3 baseCol = BoostSaturation(tex.rgb, _SaturationBoost);
                half3 shadow1 = baseCol * _ShadowColor.rgb;
                half3 shadow2 = baseCol * _Shadow2ndColor.rgb;

                half3 toonCol = lerp(shadow1, baseCol * mainLight.color.rgb, litMask);
                toonCol = lerp(toonCol, shadow2, deepMask * _ShadowStrength);

                half brightness = lerp(_LightMinLimit, _LightMaxLimit, saturate(lightValue + 0.35h));
                toonCol *= brightness;
                toonCol = lerp(toonCol, baseCol, _AsUnlit);

                half rim = pow(saturate(1.0h - dot(N, V)), _RimPower) * _RimStrength;
                toonCol += _RimColor.rgb * rim;

                return half4(saturate(toonCol), tex.a);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
