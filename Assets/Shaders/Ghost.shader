Shader "Hidden/Ghost"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
		LOD 100

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

			uniform sampler2D _MainTex;
			uniform float _GhostTime;
			uniform float _GhostFade;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = v.vertex;
				o.vertex.x = v.vertex.x + sin(v.uv.y * 250 + _GhostTime * 5.0f) / 2;

				o.vertex = UnityObjectToClipPos(o.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return fixed4(0.0f, col.g / 2.0f, col.b, col.a / 2.0f * _GhostFade);
			}
			ENDCG
		}
	}
}
