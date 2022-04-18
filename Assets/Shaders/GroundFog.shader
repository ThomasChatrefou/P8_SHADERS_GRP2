Shader"Learning/Unlit/Ground Dissolve"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Albedo", 2D) = "white" {}
        _Noise("Noise", 2D) = "black" {}
        _DistanceThreshold("Distance Threshold", Range(0.1,20.0)) = 10.0  // slider
        _NoiseFrequency("Noise Frequency", Range(0.001, 10)) = 1
        _NoiseWidth("Noise Width", Range(0.001, 10)) = 1
        _NoiseSubFractalPower("Noise Subfractral Power", Range(0.001, 10)) = 2
        _NoiseSpeedReduction("Noise Speed Reduction", Range(1, 10)) = 5
        _BorderWidth("Border Width", Range(0.001, 10)) = 2
        _BorderColor("Border Color", Color) = (0.9, 0.9, 1, 1)
        _BorderEmissionPower("Border Emission Power", Range(1, 10)) = 5
    }
    
    SubShader
    {
		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

            #include "UnityCG.cginc"
			
sampler2D _Albedo, _Noise;
float _DistanceThreshold;
float3 WS_PlayerPosition;
float _NoiseFrequency, _NoiseWidth, _NoiseSubFractalPower, _NoiseSpeedReduction;
float _BorderWidth, _BorderEmissionPower;
float4 _BorderColor;

struct vertexInput
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};
			
struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 worldSpacePos : TEXCOORD1;
};

v2f vert (vertexInput v)
{
    v2f o;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.uv = v.uv;
    o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex).xyz;
    return o;
}

float4 frag(v2f i) : SV_Target
{
    float3 playerToBorder = normalize(i.worldSpacePos - WS_PlayerPosition) * _NoiseFrequency;
    float distanceOffsetNoise = (tex2D(_Noise, WS_PlayerPosition.xz / (_NoiseSpeedReduction * _NoiseSpeedReduction) + playerToBorder.xz).r - 0.5) +
                                (tex2D(_Noise, WS_PlayerPosition.xz / _NoiseSpeedReduction + playerToBorder.xz * _NoiseSubFractalPower).r - 0.5) / 2 +
                                (tex2D(_Noise, WS_PlayerPosition.xz + playerToBorder.xz * (_NoiseSubFractalPower * _NoiseSubFractalPower)).r - 0.5) / 4;
    float distPlayerFragment = distance(WS_PlayerPosition, i.worldSpacePos);
    float isInside = step(distPlayerFragment, _DistanceThreshold + distanceOffsetNoise * _NoiseWidth);
    float isBorder = step(distPlayerFragment, _DistanceThreshold + distanceOffsetNoise * _NoiseWidth + _BorderWidth);
    return isInside * tex2D(_Albedo, i.uv) + isBorder * (1-isInside) * _BorderColor * _BorderEmissionPower;
}
            
            ENDHLSL
        }
    }
}
