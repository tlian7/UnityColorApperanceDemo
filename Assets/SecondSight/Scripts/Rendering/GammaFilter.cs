using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/GammaFilter")]
public class GammaFilter : ImageEffectBase {
	
	//public Matrix4x4 cbMatrix = Matrix4x4.identity;
	public float gammaValue = 2.2f;
	public Text gammaText;

	public int thisEye = 1; //Set which eye this filter is placed at
	public int currEye;
	private EyeToggle eyeToggle;

	void Start(){
		//Find out which eye we're using
		GameObject eyeControl = GameObject.Find("EyeControlObject");
		eyeToggle = eyeControl.GetComponent<EyeToggle> ();

	}
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		//material.SetMatrix ("_cbMatrix",cbMatrix);
		gammaValue = (float)Math.Round ((double)gammaValue, 1);
		gammaText.text = "Gamma: " + gammaValue.ToString();
		material.SetFloat ("_gammaValue", gammaValue);
		Graphics.Blit (source, destination, material);
	}
	
	void Update(){

		currEye = eyeToggle.eye;

		if ((currEye == thisEye) || (currEye == 0)) {
			if (Input.GetKeyDown ("o")) {
				gammaValue = gammaValue + 0.1f;
			}
			if (Input.GetKeyDown ("l")) {
				gammaValue = gammaValue - 0.1f;
			}
			if(Input.GetKeyDown("q")){
				gammaValue = 2.2f;
			}
			if(Input.GetKeyDown("e")){
				gammaValue = 3.2f;
			}
		}
	}
}
