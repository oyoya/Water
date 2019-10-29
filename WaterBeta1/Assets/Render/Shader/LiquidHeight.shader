Shader "Unlit/LiquidHeight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PreTex ("PreTexture", 2D) = "white" {}
		_LiquidParams ("LiquidParam", vector) = (0,0,0,0)
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
			#include "Utils.cginc"

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

			sampler2D _MainTex;
			sampler2D _PreTex;
			float4 _MainTex_ST;

			float4 _LiquidParams;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float cur = _LiquidParams.x * tex2D(_MainTex, i.uv);

				float cg = _LiquidParams.z * (DecodeHeight(tex2D(_MainTex, i.uv + float2(_LiquidParams.w, 0))) + DecodeHeight(tex2D(_MainTex, i.uv + float2(-_LiquidParams.w, 0))) +
				DecodeHeight(tex2D(_MainTex, i.uv + float2(0, _LiquidParams.w))) + DecodeHeight(tex2D(_MainTex, i.uv + float2(0, _LiquidParams.w))));
				
				float pre = _LiquidParams.y * DecodeHeight(tex2D(_PreTex, i.uv));

				cur += pre + cg;

				return EncodeHeight(cur);
			}
			ENDCG
		}
	}
}
