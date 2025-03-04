Shader"Custom/RainbowShaderWithBorder"
{
    Properties
    {
        _WaveSpeed("Wave Speed", Float) = 2.0
        _WaveAmplitude("Wave Amplitude", Float) = 0.05
        _Saturation("Saturation", Float) = 0.8
        _Brightness("Brightness", Float) = 0.5
        _BorderThickness("Border Thickness", Float) = 0.04
        _OutlineThickness("Outline Thickness", Float) = 0.02
        _BorderWaveFrequency("Border Wave Frequency", Float) = 10.0
        _BorderWaveAmplitude("Border Wave Amplitude", Float) = 0.1
        _DotSizeMin("Dot Size Min", Float) = 0.2
        _DotSizeMax("Dot Size Max", Float) = 0.4
        _DotSpacingMin("Dot Spacing Min", Float) = 0.05
        _DotSpacingMax("Dot Spacing Max", Float) = 0.2
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
float _BorderThickness;
float _OutlineThickness;
float _BorderWaveFrequency;
float _BorderWaveAmplitude;
float _DotSizeMin;
float _DotSizeMax;
float _DotSpacingMin;
float _DotSpacingMax;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed3 hsv2rgb(float3 hsv)
{
    float h = hsv.x;
    float s = hsv.y;
    float v = hsv.z;
    float c = v * s;
    float x = c * (1 - abs(fmod(h / 60.0, 2) - 1));
    float m = v - c;

    if (h < 60)
        return fixed3(c + m, x + m, m);
    else if (h < 120)
        return fixed3(x + m, c + m, m);
    else if (h < 180)
        return fixed3(m, c + m, x + m);
    else if (h < 240)
        return fixed3(m, x + m, c + m);
    else if (h < 300)
        return fixed3(x + m, m, c + m);
    else
        return fixed3(c + m, m, x + m);
}

fixed4 frag(v2f i) : SV_Target
{
    float gradient = (i.uv.x + i.uv.y) * 0.5;
    float wave = sin(gradient * 6.0 + _Time.y * _WaveSpeed) * _WaveAmplitude;
    float lerpValue = saturate(gradient + wave);
    float hue = lerpValue * 360.0;
    float3 hsv = float3(hue, _Saturation, _Brightness);
    fixed3 rgb = hsv2rgb(hsv);
    fixed4 baseColor = fixed4(rgb, 1.0);

    float dotSize = lerp(_DotSizeMax, _DotSizeMin, gradient);
    float dotSpacing = lerp(_DotSpacingMax, _DotSpacingMin, gradient);
    float2 grid = frac(i.uv / dotSpacing);
    float dist = length(grid - 0.5);
    float dot = smoothstep(dotSize, dotSize * 0.8, dist);
    fixed4 dotColor = baseColor * 0.5;
    baseColor.rgb = lerp(baseColor.rgb, dotColor.rgb, 1.0 - dot);

    float borderDist = min(min(i.uv.x, 1.0 - i.uv.x), min(i.uv.y, 1.0 - i.uv.y));
    float borderMask = smoothstep(_BorderThickness * 1.5, _BorderThickness, borderDist);
    float borderWave = sin(i.uv.x * _BorderWaveFrequency + _Time.y * _WaveSpeed) * _BorderWaveAmplitude;
    borderWave += sin(i.uv.y * _BorderWaveFrequency + _Time.y * _WaveSpeed) * _BorderWaveAmplitude;
    borderWave *= borderMask;
    fixed4 borderColor = fixed4(rgb * 0.7, 1.0);
    borderColor.rgb += borderWave;

    float outlineDist = borderDist - _OutlineThickness;
    float outlineMask = smoothstep(_BorderThickness + _OutlineThickness, _BorderThickness, outlineDist);
    fixed4 outlineColor = borderColor * 0.5;

    baseColor = lerp(baseColor, outlineColor, outlineMask);
    baseColor = lerp(baseColor, borderColor, borderMask);

    return baseColor;
}
            ENDCG
        }
    }
}
