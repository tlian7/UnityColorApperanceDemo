Shader "Hidden/Polarity Effect" {
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
uniform int _polarityBool;

fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 original = tex2D(_MainTex, i.uv);
	fixed4 output;
	if(_polarityBool == 1){
		output = (1.0f - original)-0.4;
		output[3] = 1.0f;
	}else{
		output = original;
	}
	return output;
}
ENDCG

	}
}

Fallback off

}