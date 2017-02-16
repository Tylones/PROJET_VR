// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader " Vertex Colored" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Spec Color", Color) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Range(1, 1)) = 1
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

		SubShader{
		Pass{
		Material{
		Shininess[_Shininess]
		Specular[_SpecColor]
		Emission[_Emission]
	}
		Cull Off
		ColorMaterial AmbientAndDiffuse
		Lighting On
		SeparateSpecular On
		SetTexture[_MainTex]{
		Combine texture * primary, texture * primary
	}
		SetTexture[_MainTex]{
		constantColor[_Color]
		Combine previous * constant DOUBLE, previous * constant
	}
	}
	}

		Fallback " VertexLit", 1
}