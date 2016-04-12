Shader "Secrets/Overlay"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Intensity("Multiply Intensity", range(0.0,1.0)) = 0.5
	}
	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
		}
		// No culling or depth
		Cull Off 
		ZWrite Off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#pragma target 3.0

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Intensity;

			fixed4 frag (v2f i) : SV_Target
			{
	            fixed4 finalColor;
	            finalColor.a = i.color.a;

	            fixed4 _color = tex2D(_MainTex, i.uv);
            	if(i.color.r <= 0.5)	finalColor.r = 2 * i.color.r * _color.r;
            	else				finalColor.r = 1 - 2 * (1-i.color.r) * (1-_color.r);
            	if(i.color.g <= 0.5)	finalColor.g = 2 * i.color.g * _color.g;
            	else				finalColor.g = 1 - 2 * (1-i.color.g) * (1-_color.g);
            	if(i.color.b <= 0.5)	finalColor.b = 2 * i.color.b * _color.b;
            	else				finalColor.b = 1 - 2 * (1-i.color.b) * (1-_color.b);
            	finalColor.a = _color.a;

            	float3 rgb = i.color.rgb + _Intensity *finalColor.a* (finalColor.rgb - i.color.rgb);
				return half4(rgb,i.color.a);
			}
			ENDCG
		}
	}
}
