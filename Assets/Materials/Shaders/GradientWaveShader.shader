Shader "Custom/GradientWaveShader"
{
    Properties
    {
        _Color1("Color 1", Color) = (1, 1, 1, 1) // Top-left color
        _Color2("Color 2", Color) = (0, 0, 0, 1) // Bottom-right color
        _WaveSpeed("Wave Speed", Float) = 2.0 // Slower speed for smoother effect
        _WaveAmplitude("Wave Amplitude", Float) = 0.05 // Smaller amplitude for subtlety
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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Diagonal gradient from top-left (0,0) to bottom-right (1,1)
                float gradient = (i.uv.x + i.uv.y) * 0.5; // Normalize to [0, 1]

                // Add a smooth wave effect using a sine function
                float wave = sin(gradient * 6.0 + _Time.y * _WaveSpeed) * _WaveAmplitude;

                // Combine gradient and wave smoothly
                float lerpValue = saturate(gradient + wave);

                // Blend between the two colors
                fixed4 color = lerp(_Color1, _Color2, lerpValue);
                return color;
            }
            ENDCG
        }
    }
}