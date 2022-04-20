Shader "P8_Shaders/Unlit/Chomp"
{
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue
        _Albedo("Albedo", 2D) = "white" {}
        _NormalMap("Normal map", 2D) = "white" {}
        _AmbientOcclusionMap("Ambient Occlusion Map", 2D) = "white" {}
        _MetallicSmoothnessMap("Metallic Smoothness Map", 2D) = "white" {}
    }
    
    SubShader
    {
		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            sampler2D _Albedo, _NormalMap, _AmbientOcclusionMap, _MetallicSmoothnessMap;
            
            float _ColorMultiplier;
			
			struct vertexInput
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };
			
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
};

            v2f vert (vertexInput v)
            {
                v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv.xy;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return tex2D(_Albedo, i.uv) * saturate(_ColorMultiplier);
            }
            
            ENDHLSL
        }
    }
}
