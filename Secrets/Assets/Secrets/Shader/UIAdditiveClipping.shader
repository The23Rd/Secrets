Shader "Secrets/UIAdditiveClipping" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_ClipRect ("Clipping Rect", Vector) = (5,5,5,5)

//		_StencilComp ("Stencil Comparison", Float) = 8
//		_Stencil ("Stencil ID", Float) = 0
//		_StencilOp ("Stencil Operation", Float) = 0
//		_StencilWriteMask ("Stencil Write Mask", Float) = 255
//		_StencilReadMask ("Stencil Read Mask", Float) = 255
//
//		_ColorMask ("Color Mask", Float) = 15
	}

	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

//		Stencil
//		{
//			Ref [_Stencil]
//			Comp [_StencilComp]
//			Pass [_StencilOp] 
//			ReadMask [_StencilReadMask]
//			WriteMask [_StencilWriteMask]
//		}

		Blend SrcAlpha One
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off
//		ZTest [unity_GUIZTestMode]
//		ColorMask [_ColorMask]
		
		SubShader {
			Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
//				#pragma multi_compile_fog

				#include "UnityCG.cginc"
//				#include "UnityUI.cginc"

				sampler2D _MainTex;
				fixed4 _TintColor;
				float4 _ClipRect;
				
				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
//					UNITY_FOG_COORDS(1)
				};
				
				float4 _MainTex_ST;

				v2f vert (appdata_t v)
				{
					v2f o;
					o.worldPosition = v.vertex;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
//					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag (v2f i) : SV_Target
				{
					
					fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);

					// Manually Clipping
					col.a *= (i.worldPosition.x >= _ClipRect.x);
					col.a *= (i.worldPosition.x <= _ClipRect.y);
					col.a *= (i.worldPosition.y >= _ClipRect.z);
					col.a *= (i.worldPosition.y <= _ClipRect.w);

					//Unity Native 2D Clipping
//					col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

//					UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
					return col;
				}
				ENDCG 
			}
		}	
	}
}