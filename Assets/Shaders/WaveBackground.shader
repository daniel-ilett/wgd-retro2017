/* A cool effect that creates coloured sine waves based on the screen position 
 * of fragments.
 */

Shader "PurgeTheCityDX/WaveBackground"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorA("Wave Colour A", Color) = (1, 1, 1, 1)
		_ColorB("Wave Colour B", Color) = (0, 0, 0, 1)
		_Frequency("Wave Frequency", Range(1, 50)) = 5
		_Size("Wave Size", Range(1, 100)) = 50
		_Speed("Wave Speed", Range(1, 10)) = 5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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

			uniform sampler2D _MainTex;
			uniform fixed4 _ColorA;
			uniform fixed4 _ColorB;
			uniform int _Frequency;
			uniform float _Size;
			uniform float _Speed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float s = (i.screenPos.y + sin((_Time * (_Speed / 5) + i.screenPos.x) * _Size) * (_Size / 2.0f) / _ScreenParams.y);

				s = s + 1;

				float c = floor(s * _Frequency % 2);

				return _ColorA * c + _ColorB * (1.0f - c);
			}
			ENDCG
		}
	}
}
