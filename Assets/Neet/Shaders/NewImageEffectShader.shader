Shader "Neet/UI/NewImageEffectShader"
{
	Properties
	{
		_Width("Width", Float) = 50.0
		_Height("Height", Float) = 50.0
		_Radius("Radius", Float) = 20

		_FillColor("FillColor", Color) = (1, 1, 1, 1)

		_BorderWidth("BorderWidth", Float) = 10
		_BorderColor("BorderColor", Color) = (0, 0, 0, 1)

		_InnerShadowColor("InnerShadowColor", Color) = (0, 0, 0, 1)
		_InnerShadowLength("InnerShadowLength", Float) = 6

		_OuterShadowColor("OuterShadowColor", Color) = (0, 0, 0, 1)
		_OuterShadowLength("OuterShadowLength", Float) = 2

		_BorderInnerShadowColor("BorderInnerShadowColor", Color) = (0, 0, 0, 1)
		_BorderInnerShadowLength("BorderInnerShadowLength", Float) = 1

		_OverlayColor("OverlayColor", Color) = (1, .6, 0, .3)
	}
		SubShader
	{
		Tags {"Queue" = "Transparent"
		//"IgnoreProjector" = "True" 
		"RenderType" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha
		//ColorMask RGB
		//ZWrite Off
		//Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			// #include "UnityUI.cginc"

			float _Width;
			float _Height;
			float _Radius;

			float4 _FillColor;

			float _BorderWidth;
			float4 _BorderColor;

			float _InnerShadowLength;
			float4 _InnerShadowColor;

			float _OuterShadowLength;
			float4 _OuterShadowColor;

			float4 _BorderInnerShadowColor;
			float _BorderInnerShadowLength;

			float4 _OverlayColor;



			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};

			Interpolators vert(MeshData v)
			{
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1));
				o.uv = v.uv;
				return o;
			}

			float inverseLerp(float a, float b, float v) {
				return (v - a) / (b - a);
			}
			float aa(float2 uv, float _dist) {
				float dist = length(uv);
				float pwidth = fwidth(dist);
				float alpha = smoothstep(0.5, 0.5 - pwidth * 1.5, dist);
				return alpha;
			}
			float aa2(float2 uv) {
				float dist = length(uv);
				float pwidth = length(float2(ddx(dist), ddy(dist)));
				float alpha = smoothstep(0.5, 0.5 - pwidth * 1.5, dist);
				return alpha;
			}
			float4 overlay(float4 base, float4 top) {
				float4 c = lerp(base, top, clamp(base.w, 0, 1) * clamp(top.w, 0, 1));
				c.w += base.w;

				return c;
			}
			float4 additiveOverlay(float4 base, float4 top) {
				float4 c = lerp(base, top, clamp(top.w, 0, 1));
				c.w += base.w;
				return c;
			}
			float4 aa_additiveOverlay(float2 uv, float dist, float4 base, float4 top) {
				float4 c = lerp(base, top, clamp(top.w, 0, 1) * aa(uv, dist));
				c.w += base.w;
				return c;
			}
			float4 aa_overlay(float2 uv, float dist, float4 base, float4 top) {
				float4 c = lerp(base, top, clamp(base.w, 0, 1) * clamp(top.w, 0, 1));//  *aa(uv));
				c.w += base.w;
				// c.w = aa(uv);
				return c * aa(uv, dist);
			}
			/*float4 aa_additiveOverlay(float2 uv, float4 base, float4 top) {
				float4 c = lerp(base, top, clamp(top.w, 0, 1));
				c.w += base.w;
				return c;
			}*/


			fixed4 frag(Interpolators i) : SV_Target
			{
				float2 coords = i.uv;
				coords.x *= _Width;
				coords.y *= _Height;

				float fadeStart = _Radius;//  +(_Border + _BorderFadeOut * 2);

				float2 innerPoint = float2 (clamp(coords.x, fadeStart, _Width - fadeStart), clamp(coords.y,fadeStart, _Height - fadeStart));
				float dist = distance(coords, innerPoint);

				float2 aaPoint = float2(innerPoint.x / _Width, innerPoint.y / _Height);
				float aaDist = distance(i.uv, aaPoint);

				float borderFadeOuterEdge = _Radius; // -_OuterShadowLength;
				float borderOuterEdge = borderFadeOuterEdge - _OuterShadowLength;
				float borderInnerShadowOuterEdge = borderOuterEdge - _BorderInnerShadowLength;
				float borderInnerEdge = borderOuterEdge - _BorderWidth;
				float borderInnerShadowInnerEdge = borderInnerEdge + _BorderInnerShadowLength;
				float borderFadeInnerEdge = borderInnerEdge - _InnerShadowLength;

				// inspector properties
				float4 transparent = float4(0, 0, 0, 0);

				float4 color = float4(1, 0, 0, 1);

				

				if (dist <= borderFadeInnerEdge)
				{
					color = _FillColor;
				}

				// fill shadow
				else if (dist <= borderInnerEdge) {
					float t = (dist - borderFadeInnerEdge) / _InnerShadowLength;
					float shadowAlpha = t * _InnerShadowColor.w;

					color = overlay(_FillColor, float4(_InnerShadowColor.rgb, shadowAlpha));
				}

				else if (dist <= borderOuterEdge) {

					//float4 borderBase = additiveOverlay(i.uv * 2 - 1, aaDist, _FillColor, _BorderColor);
					float4 borderBase = additiveOverlay(_FillColor, _BorderColor);
					float4 shadowBase = additiveOverlay(_BorderColor, _BorderInnerShadowColor);

					// border inner shadow A
					if (dist <= borderInnerShadowInnerEdge) {
						float t = (dist - borderInnerEdge) / _BorderInnerShadowLength;
						float alpha = (1 -t) * _BorderInnerShadowColor.w;
						//color = aa_overlay(i.uv, _BorderColor, float4(_BorderInnerShadowColor.xyz, alpha));
						color = additiveOverlay(borderBase, float4(_BorderInnerShadowColor.xyz, alpha));
					}

					// border inner shadow B
					else if (dist >= borderInnerShadowOuterEdge) {
						float t = (dist - borderInnerShadowOuterEdge) / _BorderInnerShadowLength;
						float alpha = t * _BorderInnerShadowColor.w;
						color = additiveOverlay(borderBase, float4(_BorderInnerShadowColor.xyz, alpha));
					}
					else {
						//color = aa_overlay(i.uv, _InnerShadowColor, _BorderColor);
						color = borderBase;
					}

				}
				else {// if (dist <= borderFadeOuterEdge) {
					//color = lerp(_OuterShadowColor, transparent, (dist - borderOuterEdge) / _OuterShadowLength);

					float t = (dist - borderOuterEdge) / _OuterShadowLength;
					float alpha = (1 - t) * _OuterShadowColor.w;
					color = additiveOverlay(transparent, float4(_OuterShadowColor.rgb, alpha));
				}


				if (i.uv.y > 0.5)
					color = overlay(color, _OverlayColor);
				// float alpha = inverseLerp(_Radius - _Smoothing, _Radius, dist);




				return color;
			}

			ENDCG
		}
	}
}
