// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PMShader/OutLine"
{
	Properties
	{
		_Dffuse("Diffuse",Color) = (1,1,1,1)
		_OutlineCol("OutlineCol",Color) = (0,0,0,1)
		_OutlineFactor("OutlineFactor",Range(0,10)) = 0.1
	}
	SubShader
	{

		Pass
		{
			Cull Front

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			fixed4 _OutlineCol;
			float _OutlineFactor;

			struct v2f
			{
				float4 pos:SV_POSITION;	
			};

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				float3 dir=normalize(v.vertex.xyz);
				dir   = mul ((float3x3)UNITY_MATRIX_IT_MV, dir);
            
				float2 offset = TransformViewToProjection(dir.xy);
				offset=normalize(offset);
				o.pos.xy += offset * o.pos.z *_OutlineFactor;
				return o;
			}

			fixed4 frag(v2f i):SV_Target
			{
				return _OutlineCol;
			}
			ENDCG

		}

	}
	//FallBack "Diffuse"
}
