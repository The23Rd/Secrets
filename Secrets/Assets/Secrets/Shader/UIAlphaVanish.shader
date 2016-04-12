Shader "Secrets/UIAlphaVanish"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		_VanishTex ("Vanish Alpha Texture (Gray Scale)", 2D) = "white" {}
		_AlphaVanishGate ("Alpha Vanish Gate", Range (-1.0, 2.0)) = 0.0
		_Width ("Vanish Width", Range (0.0, 0.5)) = 0.2
		_VanishPower ("Vanish Power", Range (0.5, 8.0)) = 3.0
		_AdditivePower ("Highest Additive Parameter", Range (0.0, 5.0)) = 0.5
		_AdditiveTintColor ("Additive Tint", Color) = (1.0,1.0,1.0,1.0)
	}
	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR; 
				float4 worldPosition : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _VanishTex;
			float _AlphaVanishGate;
			float _Width;
			float _VanishPower;
			float _AdditivePower;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _AdditiveTintColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = (tex2D(_MainTex, i.uv) + + _TextureSampleAdd) * i.color;
				fixed4 v_col = tex2D (_VanishTex, i.uv);

				float alpha = v_col.r;
				if (_AlphaVanishGate < 0.0)
            		return col;

//            	return fixed4 (alpha, alpha, alpha, alpha);
				float offset = alpha + 1 - _AlphaVanishGate;
				offset = offset / 2.0;
				if (offset <= 0)
					return fixed4 (col.rgb, 0);

				fixed3 myColor;
            	float4 rgba = col;

				rgba.a = col.a;
//            	myColor.r = 1 - (1-col.r) * (1-col.r);
//            	myColor.g = 1 - (1-col.g) * (1-col.g);
//            	myColor.b = 1 - (1-col.b) * (1-col.b);
				myColor = 2 * rgba.rgb;
            	if (alpha >= _AlphaVanishGate)
				{
	            	if (alpha - _AlphaVanishGate <= _Width)
	            		rgba.rgb = rgba.rgb + lerp (_AdditivePower, 0.0, pow(alpha - _AlphaVanishGate, _VanishPower) / _Width) * (myColor * _AdditiveTintColor.rgb - rgba.rgb);
				}else
				{
					if (_AlphaVanishGate - alpha <= _Width)
	            		rgba.rgb = rgba.rgb + lerp (_AdditivePower, 0.0, pow(_AlphaVanishGate - alpha, _VanishPower) / _Width) * (myColor * _AdditiveTintColor.rgb - rgba.rgb);
					rgba.a = lerp (col.a, 0.0, pow(_AlphaVanishGate - alpha, _VanishPower) / _Width);
				}


				// Unity Native UI Clipping
				rgba.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (rgba.a - 0.001);
				#endif

				return rgba;
			}
			ENDCG
		}
	}
}
