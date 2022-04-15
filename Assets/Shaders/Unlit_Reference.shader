Shader "Learning/Unlit/Reference"
{
    // Propri�t�s expos�es dans l'inspector
    // NE PAS OUBLIER DE FAIRE LE LIEN APRES DANS VOTRE CODE POUR Y ACCEDER
    Properties
    {   
        // NOM_VARIABLE("NOM_AFFICHE_DANS_L'INSPECTOR", Shaderlab type) = defaultValue 
		_WaveScale("Wave scale", Float) = 0.07
        _ReflDistort("Reflection distort", Range(0,1.5)) = 0.5  // slider
        _RefrColor("Refraction color", Color) = (0.34, 0.85, 0.92, 1) // color
        _RefrVector("Refraction color", Vector) = (1, 0, 0, 1)
        _ReflectionTex("Environment Reflection", 2D) = "white" {} // textures 
        _RefractionTex("Environment Refraction", 2D) = "black" {} 	
    }
    SubShader
    {

		Pass
        {
			HLSLPROGRAM
            #pragma vertex vert  // On d�finit le vertex shader. Nom de la fonction: vert (peut �tre chang�)
            #pragma fragment frag  // On d�finit le fragment shader. Nom de la fonction: frag (peut �tre chang�)

            #include "UnityCG.cginc"
			
			// Ecrivez ici les variables expos�es dans le bloc Properties
            float _WaveScale, _ReflDistort;
            sampler2D _ReflectionTex, _RefractionTex;
            float4 _RefrColor, _RefrVector;
			
			struct vertexInput
            {
                float4 vertex : POSITION;						
            };
            
            // Autre exemple d'un appdata contenant beaucoup plus de data 
            /*
            struct vertexInput {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 texcoord3 : TEXCOORD3;
                fixed4 color : COLOR;
            };
            */
			
            // Donn�es qui vont �tre interpol�es par le Rasterizer et qui seront en input du fragment shader
            // Chaque variable de cette struct doit �tre calcul�e dans le vertex shader !
            struct v2f   // v2f = vertex to fragment     ou p-e appel� vertexOutput
            {
	            float4 vertex : SV_POSITION; // => SV_POSITION signifie que c'est la position en clip space 
            };

            v2f vert(vertexInput v)
            {
	            v2f o;
	            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); // ETAPE OBLIGATOIRE DU VS. De l'espace objet � l'espace de clipping (�cran)
	            return o;
            }

            // FRAGMENT SHADER: calcul la couleur finale du fragment/pixel
            float4 frag(v2f i) : SV_Target
            {
	            return float4(1,0,0,0);
            }
            
            ENDHLSL
        }
    }
}
