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
			//描边只用渲染背面，挤出轮廓线，所以剔除正面
			Cull Front
			////开启深度写入，防止物体交叠处的描边被后渲染的物体盖住
			//ZWrite On
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
                float3 pos=normalize(v.vertex.xyz);
                float3 normal=normalize(v.normal);

				
				/*不明白作用*/
                //点积为了确定顶点对于几何中心的指向，判断此处的顶点是位于模型的凹处还是凸处
                float D=dot(pos,normal);
                //校正顶点的方向值，判断是否为轮廓线
                pos*=sign(D);
                //描边的朝向插值，偏向于法线方向还是顶点方向
                pos=lerp(normal,pos,_OutlineFactor);
				/*不明白作用*/



                //将顶点向指定的方向挤出
                v.vertex.xyz+=pos*_OutlineFactor;
                o.pos=UnityObjectToClipPos(v.vertex);
                return o;
			}
			fixed4 frag(v2f i):SV_Target
			{
				return _OutlineCol;
			}
			ENDCG
		}


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
}
