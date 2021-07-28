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

		_FillShadowColor("FillShadowColor", Color) = (0, 0, 0, 1)
		_FillShadowLength("FillShadowLength", Float) = 6

		_OuterShadowColor("OuterShadowColor", Color) = (0, 0, 0, 1)
		_OuterShadowLength("OuterShadowLength", Float) = 2

		_BorderShadowColor("BorderShadowColor", Color) = (0, 0, 0, 1)
		_BorderShadowLength("BorderShadowLength", Float) = 1

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

			float _FillShadowLength;
			float4 _FillShadowColor;

			float _OuterShadowLength;
			float4 _OuterShadowColor;

			float4 _BorderShadowColor;
			float _BorderShadowLength;

			float4 _OverlayColor;

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
				//if (!top.w)
					//return base;

				float4 c = lerp(base, top, clamp(top.w, 0, 1));
				c.w += base.w;
				return clamp(c, 0, 1);
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

				float2 outerShadowRange : TEXCOORD2;
				float2 borderShadowRangeB : TEXCOORD3;
			};

			Interpolators vert(MeshData v)
			{
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.uv = v.uv;

				float borderFadeOuterEdge = _Radius; // -_OuterShadowLength;
				float borderOuterEdge = borderFadeOuterEdge - _OuterShadowLength;
				float borderInnerShadowOuterEdge = borderOuterEdge - _BorderShadowLength;
				float borderInnerEdge = borderOuterEdge - _BorderWidth;
				float borderInnerShadowInnerEdge = borderInnerEdge + _BorderShadowLength;
				float borderFadeInnerEdge = borderInnerEdge - _FillShadowLength;

				o.outerShadowRange = float2(borderOuterEdge, _Radius);
				o.borderShadowRangeB = float2(borderInnerShadowOuterEdge, borderOuterEdge);

				return o;
			}

			fixed4 frag(Interpolators i) : SV_Target
			{
				/*float width = _Width;
				float height = _Height;
				float d = distance(float2(0, 0), i.uv);
				
				if (d > 0) {
					float4 pos = mul(UNITY_MATRIX_M, i.uv) / d;
					width = pos.x;
					height = pos.y;
				}*/


				float2 coords = i.uv;
				coords.x *= _Width;
				coords.y *= _Height;
				//coords.x *= width;
				//coords.y *= height;

				float fadeStart = _Radius;//  +(_Border + _BorderFadeOut * 2);

				float2 innerPoint = float2 (clamp(coords.x, fadeStart, _Width - fadeStart), clamp(coords.y,fadeStart, _Height - fadeStart));
				float dist = distance(coords, innerPoint);

				float2 aaPoint = float2(innerPoint.x / _Width, innerPoint.y / _Height);
				float aaDist = distance(i.uv, aaPoint);

				float borderFadeOuterEdge = _Radius; // -_OuterShadowLength;
				float borderOuterEdge = borderFadeOuterEdge - _OuterShadowLength;
				float borderInnerShadowOuterEdge = borderOuterEdge - _BorderShadowLength;
				float borderInnerEdge = borderOuterEdge - _BorderWidth;
				float borderInnerShadowInnerEdge = borderInnerEdge + _BorderShadowLength;
				float borderFadeInnerEdge = borderInnerEdge - _FillShadowLength;

				// inspector properties
				float4 transparent = float4(0, 0, 0, 0);

				float4 color = float4(1, 0, 0, 1);

				//float aa_mask = saturate((borderOuterEdge / dist) / fwidth(length(i.uv)));

				float pwidth = fwidth(aaDist);
				float aa_mask = smoothstep(0.5, 0.5 - pwidth * 1.5, aaDist);

				// fill
				float fillMask = (dist <= borderOuterEdge) * _FillColor.a;// *aa_mask;
				float4 fillColor = float4(_FillColor.rgb, fillMask);


				// border
				//float innerShadowMask = dist >= borderFadeInnerEdge && dist <= borderInnerEdge;
				float borderMask = dist >= borderInnerEdge && dist <= borderOuterEdge;
				borderMask *= _BorderColor.a;
				float4 borderColor = float4(_BorderColor.rgb, borderMask);

				// border inner shadow
				// get the distance to the nearest border edge to find shadow alpha
				float borderInnerEdgeDist = distance(dist, borderInnerEdge);
				float borderOuterEdgeDist = distance(dist, borderOuterEdge);
				float borderEdgeDist = min(distance(dist, borderInnerEdge), distance(dist, borderOuterEdge));

				// float borderEdgeDist = distance(dist, (clamp(dist, borderInnerEdge, borderOuterEdge)));
				float borderShadowAlpha = (1 - saturate(borderEdgeDist / _BorderShadowLength)) * _BorderShadowColor.a;
				float borderShadowMask = borderShadowAlpha * borderMask;
				float4 borderShadowColor = float4(_BorderShadowColor.rgb, borderShadowMask);

				// fill shadow
				// get distance to inner border edge to find fill shadow alpha
				float borderDist = distance(dist, borderInnerEdge);
				float fillShadowMask = (1 - saturate(borderDist / _FillShadowLength)) * _FillShadowColor.a * fillMask;
				float4 fillShadowColor = float4(_FillShadowColor.rgb, fillShadowMask);

				// outer shadow
				// get distance from outer border edge to find outer shadow alpha
				float edgeDist = distance(dist, borderOuterEdge);
				float outerShadowMask = dist >= borderOuterEdge && dist <= _Radius;
				outerShadowMask *= (1 - saturate(edgeDist / _OuterShadowLength));
				float4 outerShadowColor = float4(_OuterShadowColor.rgb, outerShadowMask);

				float overlayMask = (i.uv.y > .5) * fillMask * _OverlayColor.a;
				float4 overlayColor = float4(_OverlayColor.rgb, overlayMask);

				color = fillColor;
				color = additiveOverlay(color, fillShadowColor);
				color = additiveOverlay(color, borderColor);
				color = additiveOverlay(color, borderShadowColor);
				color = additiveOverlay(color, outerShadowColor);
				color = additiveOverlay(color, overlayColor);
				return color;
			}

			ENDCG
		}
	}
}
