/*	Credit for RGB<->HSV transformations: http://www.chilliant.com/rgb2hsv.html
 */

Shader "PurgeTheCityDX/RainbowBlend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlendAmount("Blend %", Range(0, 1)) = 0
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
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
				float2 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform float _BlendAmount;

			// Following functions define RGB<->HSV colour space transformations.
			float Epsilon = 1e-10;

			float3 HUEtoRGB(float h)
			{
				float r = abs(h * 6 - 3) - 1;
				float g = 2 - abs(h * 6 - 2);
				float b = 2 - abs(h * 6 - 4);

				return saturate(float3(r, g, b));
			}

			/*
			float3 RGBtoHCV(float3 RGB)
			{
				float4 p = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : 
					float4(RGB.gb, 0.0, -1.0 / 3.0);
				float4 q = (RGB.r < p.x) ? float4(p.xyw, RGB.r) : float4(RGB.r, p.yzx);
				float c = q.x - min(q.w, q.y);
				float h = abs((q.w - q.y) / (6 * c + Epsilon) + q.z);
				return float3(h, c, q.x);
			}

			float3 RGBtoHSV(float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float s = HCV.y / (HCV.z + Epsilon);

				return float3(HCV.x, s, HCV.z);
			}

			float3 HSVtoRGB(float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);

				return ((RGB - 1) * HSV.y + 1) * HSV.z;
			}
			*/

			// Blend each pixel with a hue based on position.
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float param = _Time * 10.0f + i.screenPos.x + i.screenPos.y / 2.0f;

				float4 rainbow = fixed4(HUEtoRGB(param % 1.0f), col.a);
				fixed4 result =  rainbow * _BlendAmount + col * (1 - _BlendAmount);

				//result.a = 1.0f;

				//result = fixed4(col.a, col.a, col.a, 1.0f);

				return result;
			}
			ENDCG
		}
	}
}
