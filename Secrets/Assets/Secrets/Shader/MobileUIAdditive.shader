﻿Shader "Secrets/MobileUIAdditive" {
	Properties {
		[PerRendererData] _MainTex ("Particle Texture", 2D) = "white" {}

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		
		_ColorMask ("Color Mask", Float) = 15
		
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}

	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader {
			 Stencil
		    {
		        Ref [_Stencil]
		        Comp [_StencilComp]
		        Pass [_StencilOp] 
		        ReadMask [_StencilReadMask]
		        WriteMask [_StencilWriteMask]
		    }

			Pass {
				SetTexture [_MainTex] {
					combine texture * primary
				}
			}
		}
	}
}