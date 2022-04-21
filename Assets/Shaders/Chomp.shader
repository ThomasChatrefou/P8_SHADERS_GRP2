Shader "P8_Shaders/Lit/Chomp"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Albedo", 2D) = "white" {}
        _AmbientOcclusionMap("Ambient Occlusion Map", 2D) = "white" {}
        _MetallicSmoothnessMap("Metallic Smoothness Map", 2D) = "white" {}
        [HDR]_Color("Color Tint", Color) = (1, 1, 1, 1)
        [HDR]_DamagedColor("Damaged Color", Color) = (1, 0, 0, 0)
        [HDR]_EmissiveGumColor("Emissive Gum Color", Color) = (1,1,0,1)
        //_PercentPickedGum("Percent Picked Gum", Range(0, 1)) = 0
    }
    
    SubShader
    {
		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            sampler2D _Albedo, _NormalMap, _AmbientOcclusionMap, _MetallicSmoothnessMap;
            float4 _Color;
            float4 _DamagedColor;
            float _ColorMultiplier;
            float _PercentPickedGum;
            float4 _EmissiveGumColor;
			
			struct vertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv : TEXCOORD0;
            };
			
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldSpaceNormal : TEXCOORD1;
};

            v2f vert (vertexInput v)
            {
                v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv.xy;
                o.worldSpaceNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0)).xyz);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // lighting
                float3 N = normalize(i.worldSpaceNormal);
	            float receivedLight = saturate(dot(N, GetMainLight().direction.xyz));
                float4 ao = tex2D(_AmbientOcclusionMap, i.uv);
	            float4 light = float4(receivedLight, receivedLight, receivedLight, 1) * float4(GetMainLight().color, 1)
                            + float4(ao.xyz * SampleSH(half4(i.worldSpaceNormal, 1)), 1);
                
                float colorMultiplier = 1 - saturate(_ColorMultiplier);
    
	            return tex2D(_Albedo, i.uv) * light * _Color
                    * lerp(float4(1, 1, 1, 1), _DamagedColor, colorMultiplier)
                    * lerp(float4(1, 1, 1, 1), _EmissiveGumColor, _PercentPickedGum);
}
            
            ENDHLSL
        }
    }
}
