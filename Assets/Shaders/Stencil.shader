Shader "Siis/Unlit/Stencil" {

	Properties {
		_Background ("Background Color", Color) = (0,0,0,1)
		_BackgroundTexture ("Background Texture", 2D) = "white" {}

		[Header(Stencil)]
        [IntRange]_Stencil("Stencil", Range(0,255)) = 2
        [IntRange]_ReadMask ("ReadMask", Range(0,255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 3
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0

		[Header(Blending)]
        [Enum(UnityEngine.Rendering.BlendMode)] _Blend("Blend First", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _Blend2("Blend Second", Float) = 10

	}

	SubShader {

		Tags { "RenderType"="Sprite" "Queue"="Transparent+1" }
		LOD 100
		Blend [_Blend] [_Blend2]
		ZWrite Off

		Pass {
		    Stencil {
                Ref [_Stencil]
                ReadMask [_ReadMask]
                WriteMask [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
            }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldNormal : NORMAL;
			};

			float4 _Background;
			sampler2D _BackgroundTexture;
			float4 _BackgroundTexture_ST;

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _BackgroundTexture);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				fixed4 bg = tex2D (_BackgroundTexture, i.uv) * _Background;
				return bg;
			}
			ENDCG
		}
	}

}
