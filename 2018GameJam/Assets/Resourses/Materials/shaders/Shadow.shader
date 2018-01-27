Shader "Shadow" {

	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_ShadowTex("Texture", 2D) = "white" {}
		_Resolution("Resolution", Vector) = ( 0.0, 0.0, 0.0, 0.0 )
		_Timer("Timer", Float) = 0
	}

	SubShader{
		LOD 100
		Cull Off


		Tags{ "RenderType" = "Opaque" }

		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			float2 _Resolution;

			float _Timer;
			bool wave;

			sampler2D _MainTex;
			sampler2D _ShadowTex;
			sampler2D _WaveTex;

			fixed4 blurShadow(v2f_img i) {
				fixed4 color = { 0.0, 0.0, 0.0, 0.0 };
				float size = 20;
				int minY = int(size / 2);
				int minX = int(size / 2);
				for(int y = -minY; y <= minY; y++) {
					for(int x = -minX; x <= minX; x++) {
						float2 nUV = i.uv;
						nUV.x += x * (1.0 / _Resolution.x);
						nUV.y += y * (1.0 / _Resolution.y);
						color += tex2D(_ShadowTex, nUV);
					}
				}
				color.r /= (size * size);
				color.g /= (size * size);
				color.b /= (size * size);
				color.a /= (size * size);

				return color;
			}

			fixed4 frag(v2f_img i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv);
				fixed4 shadow = blurShadow(i);

				float2 newUV = i.uv.xy / _Resolution.xy;
				newUV.x *= _Resolution.x / _Resolution.y;
				float d = 0.0;
				float d2 = 0.0;

				newUV = (i.uv * 2.0 - 1.0);

				d = length(abs(newUV)) + _Timer;
				fixed4 frag_color = cos(d) * shadow;
				frag_color.w = 1.0;

				d2 = length(abs(newUV)) + 2.85;
				fixed4 frag_color2 = min(0.5, sin(d2)) * 4;
				frag_color2.w = 1.0;

				//frag_color *= float4( 1.0, 1.0, 1.0, 1.0 );

				float4 brightness;
				brightness.r = min(max(frag_color.r, frag_color2.r), 1.0);
				brightness.g = min(max(frag_color.r, frag_color2.r), 1.0);
				brightness.b = min(max(frag_color.r, frag_color2.r), 1.0);
				brightness.a = 1.0;

				return color * brightness;
				//fixed4 color1 = color * frag_color2;
				//fixed4 color2 = color * shadow * frag_color;
				//return color1 + color2;
				//return frag_color2 * 5;
			}
			
			ENDCG
		}
	}
}