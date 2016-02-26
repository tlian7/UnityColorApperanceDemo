Shader "Hidden/GammaCorrection" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float _gammaValue;
    
fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 color = tex2D(_MainTex, i.uv);
	color.rgb = pow(color.rgb,_gammaValue/2.2);
	return color;
}
ENDCG

	}
}

Fallback off

}