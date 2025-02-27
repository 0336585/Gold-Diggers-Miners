Shader "Custom/RainbowShader"
{
    Properties
    {
        _WaveSpeed("Wave Speed", Float) = 2.0 // Speed of the wave animation
        _WaveAmplitude("Wave Amplitude", Float) = 0.05 // Intensity of the wave
        _Saturation("Saturation", Float) = 0.8 // Controls color vibrancy
        _Brightness("Brightness", Float) = 0.5 // Controls overall darkness
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

            float _WaveSpeed;
            float _WaveAmplitude;
            float _Saturation;
            float _Brightness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Function to convert HSV to RGB
            fixed3 hsv2rgb(float3 hsv)
            {
                float h = hsv.x;
                float s = hsv.y;
                float v = hsv.z;
                float c = v * s;
                float x = c * (1 - abs(fmod(h / 60.0, 2) - 1));
                float m = v - c;

                if (h < 60) return fixed3(c + m, x + m, m);
                else if (h < 120) return fixed3(x + m, c + m, m);
                else if (h < 180) return fixed3(m, c + m, x + m);
                else if (h < 240) return fixed3(m, x + m, c + m);
                else if (h < 300) return fixed3(x + m, m, c + m);
                else return fixed3(c + m, m, x + m);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Diagonal gradient from top-left (0,0) to bottom-right (1,1)
                float gradient = (i.uv.x + i.uv.y) * 0.5; // Normalize to [0, 1]

                // Add a smooth wave effect using a sine function
                float wave = sin(gradient * 6.0 + _Time.y * _WaveSpeed) * _WaveAmplitude;

                // Combine gradient and wave smoothly
                float lerpValue = saturate(gradient + wave);

                // Create a rainbow effect using HSV to RGB conversion
                float hue = lerpValue * 360.0; // Cycle through hues (0-360)
                float3 hsv = float3(hue, _Saturation, _Brightness); // HSV color
                fixed3 rgb = hsv2rgb(hsv); // Convert to RGB

                return fixed4(rgb, 1.0); // Output the color
            }
            ENDCG
        }
    }
}