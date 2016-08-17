Shader "Transparent/Diffuse_ZWrite" {
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader{
		Tags
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		LOD 0

		// extra pass that renders to depth buffer only
		Pass
		{
			ColorMask 0
			Cull Off
			Lighting Off
			ZWrite On
			Blend One OneMinusSrcAlpha
		}

		// paste in forward rendering passes from Transparent/Diffuse
		UsePass "Transparent/Diffuse/FORWARD"
	}

		Fallback "Transparent/VertexLit"
}