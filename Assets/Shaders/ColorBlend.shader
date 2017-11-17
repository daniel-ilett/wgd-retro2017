Shader "PurgeTheCityDX/ColourBlend"
{
	Properties
	{
		_Color("Blend Color", Color) = (1, 1, 1, 1)
		_BlendAmount("Blend %", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

		// GrabPass gets whatever pixels are already in the image just before 
		// this render pass.
		GrabPass
		{
			"_BackgroundTexture"
		}

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
				float4 grabPos : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform float4 _Color;
			uniform float _BlendAmount;

			// The portion of the image already drawn.
			uniform sampler2D _BackgroundTexture;

			fixed4 frag(v2f i) : SV_Target
			{
				// Sample the previous render pass.
				fixed4 oldPixels = tex2D(_BackgroundTexture, i.grabPos);

				return fixed4(_Color.rgb, oldPixels.a) * _BlendAmount + oldPixels * (1 - _BlendAmount);
			}
			ENDCG
		}
	}
}
