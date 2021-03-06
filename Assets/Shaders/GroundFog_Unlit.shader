Shader "P8_Shaders/Unlit/Ground_Dissolve"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Albedo", 2D) = "white" {}
        _Noise("Noise", 2D) = "black" {}
        _DistanceThreshold("Distance Threshold", Range(0.1,50.0)) = 10.0  // slider
        _NoiseFrequency("Noise Frequency", Range(0.001, 10)) = 1
        _NoiseWidth("Noise Width", Range(0.001, 10)) = 1
        _NoiseSubFractalPower("Noise Subfractral Power", Range(0.001, 10)) = 2
        _NoiseSpeedReduction("Noise Speed Reduction", Range(1, 10)) = 5
        _BorderWidth("Border Width", Range(0.001, 10)) = 2
        [HDR]_BorderColor("Border Color", Color) = (4.5, 4.5, 5, 1)
    }
    
    SubShader
    {
    
        //Tags {"RenderType"="Opaque"}
        //Tags { "RenderPipeline" = "UniversalRenderPipeline" }
    
		Pass
        {
            //Tags {"LightMode"="ForwardBase"}
                
			CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag
            //#pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
			
sampler2D _Albedo, _Noise;
float _DistanceThreshold;
float3 WS_PlayerPosition;
float _NoiseFrequency, _NoiseWidth, _NoiseSubFractalPower, _NoiseSpeedReduction;
float _BorderWidth, _BorderEmissionPower;
float4 _BorderColor;

struct vertexInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};
			
struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 worldSpacePos : TEXCOORD1;
    float3 worldSpaceNormal : TEXCOORD2;
};

v2f vert (vertexInput v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex).xyz;
    o.worldSpaceNormal = normalize(UnityObjectToWorldNormal(float4(v.normal, 0)).xyz);
    return o;
}

float4 frag(v2f i) : SV_Target
{

    // computing noised border
    float3 playerToBorder = normalize(i.worldSpacePos - WS_PlayerPosition) * _NoiseFrequency;
    float distanceOffsetNoise = (tex2D(_Noise, WS_PlayerPosition.xz / (_NoiseSpeedReduction * _NoiseSpeedReduction) + playerToBorder.xz).r - 0.5) +
                                (tex2D(_Noise, WS_PlayerPosition.xz / _NoiseSpeedReduction + playerToBorder.xz * _NoiseSubFractalPower).r - 0.5) / 2 +
                                (tex2D(_Noise, WS_PlayerPosition.xz + playerToBorder.xz * (_NoiseSubFractalPower * _NoiseSubFractalPower)).r - 0.5) / 4;
    float distPlayerFragment = distance(WS_PlayerPosition, i.worldSpacePos);
    float maxDistance = _DistanceThreshold + distanceOffsetNoise * _NoiseWidth;
    if (distPlayerFragment > maxDistance + _BorderWidth)
        discard;
    float isInside = step(distPlayerFragment, maxDistance);
    float isBorder = step(distPlayerFragment, maxDistance + _BorderWidth);
    
    // lighting
    float3 N = normalize(i.worldSpaceNormal);
    float receivedLight = saturate(dot(N, _WorldSpaceLightPos0.xyz));
    
    return isInside /* * float4(receivedLight, receivedLight, receivedLight, 1) * _LightColor0*/ * tex2D(_Albedo, i.uv) + isBorder * (1 - isInside) * _BorderColor;
    //return float4(N, 1);
}
            
            ENDCG
        }
    }
}
