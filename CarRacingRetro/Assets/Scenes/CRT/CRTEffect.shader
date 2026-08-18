Shader "Hidden/CRTEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.3
        _ScanlineCount ("Scanline Count", Float) = 400
        _Curvature ("Curvature", Range(0,1)) = 0.15
        _VignetteAmount ("Vignette Amount", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off Cull Off

        Pass
        {
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

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _ScanlineIntensity;
            float _ScanlineCount;
            float _Curvature;
            float _VignetteAmount;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float2 CurveUV(float2 uv)
            {
                uv = uv * 2.0 - 1.0;
                float2 offset = abs(uv.yx) / float2(6.0 / _Curvature, 4.0 / _Curvature);
                uv = uv + uv * offset * offset;
                uv = uv * 0.5 + 0.5;
                return uv;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = CurveUV(IN.uv);

                if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0)
                    return half4(0, 0, 0, 1);

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                float scanline = sin(IN.uv.y * _ScanlineCount * 3.14159) * 0.5 + 0.5;
                col.rgb *= lerp(1.0, scanline, _ScanlineIntensity);

                float2 vigUV = IN.uv * 2.0 - 1.0;
                float vig = 1.0 - dot(vigUV, vigUV) * _VignetteAmount;
                col.rgb *= vig;

                return col;
            }
            ENDHLSL
        }
    }
}