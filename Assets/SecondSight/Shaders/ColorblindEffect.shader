Shader "Hidden/Colorblind Effect" {
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
uniform fixed4x4 _cbMatrix;

// Un-scaled SPD (old)
//const fixed3x3 m_rgb2lms = fixed3x3(
//   31.8700,    8.9275,    0.9276,
//   73.7767,   81.2659,    1.5408,
//    7.6627,   12.6718,   55.5929
//   );
//   
//const fixed3x3 m_lms2rgb = fixed3x3(
//    0.0420,   -0.0045,   -0.0006,
//   -0.0382,    0.0165,    0.0002,
//    0.0029,   -0.0031,    0.0180
//);
 
// Scaled SPD
 const fixed3x3 m_rgb2lms = fixed3x3(
   20.9464,    5.8675,    0.6096,
   46.8810,   51.6400,    0.9791,
    5.5024,    9.0994,   39.9202
   );
   
   const fixed3x3 m_lms2rgb = fixed3x3(
    0.0639,   -0.0071,   -0.0008,
   -0.0581,    0.0259,    0.0003,
    0.0044,  -0.0049,    0.0251
);

//Take the 3x3 from the 4x4 passed from the script. Unity script only deals with 4x4 matrices.  
const fixed3x3 cbTransform = fixed3x3(_cbMatrix); 

// Note:
// For dichromat simulation, we apply the transform:
// old RGB * m_rgb2lms * cbTransform * m_lms2rgb = new RGB  
// For color anomalous, we want to use this same shader but we don't need to transform to rgb2lms. So what we do instead
// is pass in a cbTransform like so:
// old RGB * m_rgb2lms * m_lms2rgb * anomalousTransform * m_rgb2lms * lms2rgb = new RGB
// Therefore our cbTransform matrix = m_lms2rgb * anomalousTransform * m_rgb2lms which is precalculated in MATLAB
// See "UnityColorAnomalousPipeline.m"

fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 original = tex2D(_MainTex, i.uv);
	fixed3 lms = mul(original.rgb,m_rgb2lms);
	lms = mul(lms,cbTransform);
	fixed4 output = fixed4(mul(lms,m_lms2rgb),original.a);
	// output = pow(output,1f/2.2f);
	return output;
}
ENDCG

	}
}

Fallback off

}
