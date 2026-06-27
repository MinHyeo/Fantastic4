Shader "Custom/RangeVisualizer"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 1, 0, 0.6)
        _BaseMap ("Base Map", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Overlay"
        }

        Pass
        {
            Name "SeeThroughLine"

            Tags
            {
                "LightMode" = "UniversalForward"
            }

            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                Varyings output;

                // 오브젝트 공간 좌표를 화면 클립 좌표로 변환합니다.
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);

                // LineRenderer의 startColor/endColor 값을 셰이더로 전달합니다.
                output.color = input.color;

                // 텍스처 타일링/오프셋을 적용한 UV를 계산합니다.
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                // 기본 텍스처 색상을 가져옵니다.
                half4 textureColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);

                // 머티리얼 색상, LineRenderer 색상, 텍스처 색상을 모두 곱합니다.
                half4 finalColor = textureColor * _BaseColor * input.color;

                return finalColor;
            }

            ENDHLSL
        }
    }
}
