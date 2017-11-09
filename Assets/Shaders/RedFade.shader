Shader "PurgeTheCityDX/RedFade"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BlendAmount("Blend %", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

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
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform sampler2D _MainTex;
			uniform float _BlendAmount;

			// Blend each pixel with a hue based on position.
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				return fixed4(1, 0, 0, col.a) * _BlendAmount + col * (1 - _BlendAmount);
			}
			ENDCG
		}
	}
}
