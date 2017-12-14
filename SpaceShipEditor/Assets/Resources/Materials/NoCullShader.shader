Shader "Custom/NoCullShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG


			Pass
		{
			CGPROGRAM
#pragma vertex MyVert

#include "UnityCG.cginc"

			struct appdata
		{
			float4 vertex : POSITION;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
		};

		// our own matrix
		float4x4 MyXformMat;  // our own transform matrix!!
		fixed4   MyColor;

		v2f MyVert(appdata v)
		{
			v2f o;

			// Can use one of the followings:
			// o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);  // Camera + GameObject transform TRS

			o.vertex = mul(MyXformMat, v.vertex);  // use our own transform matrix!
												   // MUST apply before camera!

			o.vertex = mul(UNITY_MATRIX_VP, o.vertex);   // camera transform only                

			return o;
		}
			ENDCG
		}

	}
	FallBack "Diffuse"
}
