Shader "Secrets/AlphaVanish"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VanishTex ("Vanish Alpha Texture (Gray Scale)", 2D) = "white" {}
		_AlphaVanishGate ("Alpha Vanish Gate", Range (-1.0, 2.0)) = 0.0
		_Width ("Vanish Width", Range (0.1, 0.5)) = 0.2
		_VanishPower ("Vanish Power", Range (0.5, 8.0)) = 3.0
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
	    Blend SrcAlpha OneMinusSrcAlpha
	    Cull Off
	    LOD 200

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _VanishTex;
			float _AlphaVanishGate;
			float _Width;
			float _VanishPower;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
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



				if (alpha >= _AlphaVanishGate)
				{
					rgba.a = col.a;
					if(col.r <= 0.5)	myColor.r = 2 * col.r * col.r;
	            	else				myColor.r = 1 - 2 * (1-col.r) * (1-col.r);
	            	if(col.g <= 0.5)	myColor.g = 2 * col.g * col.g;
	            	else				myColor.g = 1 - 2 * (1-col.g) * (1-col.g);
	            	if(col.b <= 0.5)	myColor.b = 2 * col.b * col.b;
	            	else				myColor.b = 1 - 2 * (1-col.b) * (1-col.b);

	            	if (alpha - _AlphaVanishGate <= _Width)
	            		rgba.rgb = rgba.rgb + lerp (0.2, 0.0, pow(alpha - _AlphaVanishGate, _VanishPower) / _Width) * myColor;
				}else
				{
					rgba.a = lerp (col.a, 0.0, pow(_AlphaVanishGate - alpha, _VanishPower) / _Width);
				}
				return rgba;
			}
			ENDCG
		}
	}
}
