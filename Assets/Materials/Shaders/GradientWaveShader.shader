Shader "Custom/GradientWaveShaderWithBorder"
{
    Properties
    {
        _Color1("Color 1", Color) = (1, 1, 1, 1)
        _Color2("Color 2", Color) = (0, 0, 0, 1)
        _WaveSpeed("Wave Speed", Float) = 2.0
        _WaveAmplitude("Wave Amplitude", Float) = 0.05
        _DotSizeMin("Dot Size Min", Float) = 0.2
        _DotSizeMax("Dot Size Max", Float) = 0.4
        _DotSpacingMin("Dot Spacing Min", Float) = 0.05
        _DotSpacingMax("Dot Spacing Max", Float) = 0.2
        _BorderThickness("Border Thickness", Float) = 0.04
        _OutlineThickness("Outline Thickness", Float) = 0.02
        _BorderWaveFrequency("Border Wave Frequency", Float) = 10.0 // Controls how many waves appear on the border
        _BorderWaveAmplitude("Border Wave Amplitude", Float) = 0.1 // Controls the intensity of the wave effect
        _UnscaledTime("Unscaled Time", Float) = 0.0 // New property for unscaled time
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            float _WaveSpeed;
            float _WaveAmplitude;
            float _DotSizeMin;
            float _DotSizeMax;
            float _DotSpacingMin;
            float _DotSpacingMax;
            float _BorderThickness;
            float _OutlineThickness;
            float _BorderWaveFrequency;
            float _BorderWaveAmplitude;
            float _UnscaledTime; // Unscaled time property

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float gradient = (i.uv.x + i.uv.y) * 0.5;
                float wave = sin(gradient * 6.0 + _UnscaledTime * _WaveSpeed) * _WaveAmplitude;
                float lerpValue = saturate(gradient + wave);
                fixed4 gradientColor = lerp(_Color1, _Color2, lerpValue);

                float dotSize = lerp(_DotSizeMax, _DotSizeMin, gradient);
                float dotSpacing = lerp(_DotSpacingMax, _DotSpacingMin, gradient);

                float2 grid = frac(i.uv / dotSpacing);
                float dist = length(grid - 0.5);
                float dot = smoothstep(dotSize, dotSize * 0.8, dist);

                fixed4 dotColor = gradientColor * 0.5;
                gradientColor.rgb = lerp(gradientColor.rgb, dotColor.rgb, 1.0 - dot);

                // Calculate border distance and mask
                float borderDist = min(min(i.uv.x, 1.0 - i.uv.x), min(i.uv.y, 1.0 - i.uv.y));
                float borderMask = smoothstep(_BorderThickness * 1.5, _BorderThickness, borderDist);

                // Add a wave effect to the border
                float borderWave = sin(i.uv.x * _BorderWaveFrequency + _UnscaledTime * _WaveSpeed) * _BorderWaveAmplitude;
                borderWave += sin(i.uv.y * _BorderWaveFrequency + _UnscaledTime * _WaveSpeed) * _BorderWaveAmplitude;
                borderWave *= borderMask; // Only apply the wave effect to the border

                // Modulate the border color with the wave effect
                fixed4 borderColor = lerp(_Color1, _Color2, gradient);
                borderColor.rgb += borderWave; // Add the wave effect to the border color

                // Darken the border color
                borderColor.rgb *= 0.7;

                // Calculate outline distance and mask
                float outlineDist = borderDist - _OutlineThickness;
                float outlineMask = smoothstep(_BorderThickness + _OutlineThickness, _BorderThickness, outlineDist);

                // Define outline color (darker than the border)
                fixed4 outlineColor = borderColor * 0.5;

                // Apply outline first, then border, then gradient
                gradientColor = lerp(gradientColor, outlineColor, outlineMask); // Apply outline
                gradientColor = lerp(gradientColor, borderColor, borderMask); // Apply border

                return gradientColor;
            }
            ENDCG
        }
    }
}
