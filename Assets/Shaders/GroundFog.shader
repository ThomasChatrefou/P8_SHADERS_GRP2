Shader "Learning/Unlit/Ground Dissolve"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Albedo", 2D) = "white" {}
        _DistanceThreshold("Distance Threshold", Range(0.1,20.0)) = 10.0  // slider
        _PlayerPosition("Player Position", Vector) = (0, 1, 0)
    }
    
    SubShader
    {
		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

            #include "UnityCG.cginc"
			
sampler2D _Albedo;
float _DistanceThreshold;
float3 _PlayerPosition;

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
    return step(distance(_PlayerPosition, i.worldSpacePos), _DistanceThreshold) * tex2D(_Albedo, i.uv);
}
            
            ENDHLSL
        }
    }
}
