Shader"P8_Shaders/Lit/Chomp"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Albedo", 2D) = "white" {}
        _AmbientOcclusionMap("Ambient Occlusion Map", 2D) = "white" {}
        _MetallicSmoothnessMap("Metallic Smoothness Map", 2D) = "white" {}
        [HDR]_DamagedColor("Damaged Color", Color) = (1, 0, 0, 0)
    }
    
    SubShader
    {
		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            
            sampler2D _Albedo, _NormalMap, _AmbientOcclusionMap, _MetallicSmoothnessMap;
            float4 _DamagedColor;
            float _ColorMultiplier;
			
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
                float receivedLight = saturate(dot(N, _WorldSpaceLightPos0.xyz));
                float4 ao = tex2D(_AmbientOcclusionMap, i.uv);
                float ambientPower = (UNITY_LIGHTMODEL_AMBIENT.r + UNITY_LIGHTMODEL_AMBIENT.g + UNITY_LIGHTMODEL_AMBIENT.b) / 3;
                float isDirectionalLightShinier = step(ao.r * ambientPower, receivedLight);
                float4 light = isDirectionalLightShinier * float4(receivedLight, receivedLight, receivedLight, 1) * _LightColor0
                            + (1 - isDirectionalLightShinier) * float4(ao.xyz, 1) * UNITY_LIGHTMODEL_AMBIENT;
                
                float colorMultiplier = 1 - saturate(_ColorMultiplier);
    
	            return tex2D(_Albedo, i.uv) * light * lerp(float4(1,1,1,1), _DamagedColor, colorMultiplier);
}
            
            ENDHLSL
        }
    }
}
