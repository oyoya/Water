Shader "Unlit/LiquidNormal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Utils.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float lh = DecodeHeight(tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x, 0.0)));
				float rh = DecodeHeight(tex2D(_MainTex, i.uv + float2(-_MainTex_TexelSize.x, 0.0)));
				float fh = DecodeHeight(tex2D(_MainTex, i.uv + float2(0.0, _MainTex_TexelSize.x)));
				float bh = DecodeHeight(tex2D(_MainTex, i.uv + float2(0.0, -_MainTex_TexelSize.x)));

				float3 normal = normalize(float3(rh - lh, bh - fh, 4 * _MainTex_TexelSize.x));

#if defined(UNITY_NO_DXT5nm)
				return float4(normal * 0.5 + 0.5, 1.0);
#elif UNITY_VERSION > 2018
				return float4(normal.x * 0.5 + 0.5, normal.y * 0.5 + 0.5, 0.0, 1.0);
#else
				return float4(0.0, normal.y * 0.5 + 0.5, 0.0, normal.x * 0.5 + 0.5);
#endif
			}
			ENDCG
		}
	}
}
