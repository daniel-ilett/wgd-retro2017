Shader "PurgeTheCityDX/Postprocess/CRTScreen"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Brightness("Brightness", Float) = 0
		_Contrast("Contrast", Float) = 0
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			// OpenGL ES 2.0 can't use non-square matrices.
			#pragma exclude_renderers gles

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
				float4 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform float _Brightness;
			uniform float _Contrast;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			
				fixed2 ps = i.screenPos.xy * _ScreenParams.xy / i.screenPos.w;

				float4 r = float4(col.r, col.g / 2, 0, 1);
				float4 g = float4(0, col.g, col.b / 2, 1);
				float4 b = float4(col.r / 2, 0, col.b, 1);
				float3x4 colormap = float3x4(r, g, b);

				float4 wh = float4(1, 1, 1, 1);
				float4 bl = float4(0, 0, 0, 1);

				float3x4 scanlineMap = float3x4(wh, wh, bl);

				fixed4 returnVal =  colormap[(uint)ps.x % 3] * scanlineMap[(uint)ps.y % 3];

				returnVal += (_Brightness / 255);

				returnVal = returnVal - _Contrast * (returnVal - 1.0) * returnVal * (returnVal - 0.5);

				return returnVal;
			}
			ENDCG
		}
	}
}
