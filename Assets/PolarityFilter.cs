using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/PolarityFilter")]
public class PolarityFilter : ImageEffectBase {
	
	public int thisEye = 1; //Set which eye this filter is placed at
	public int currEye;
	private EyeToggle eyeToggle;
	public int polarityOn;
	
	void Start(){
		//Find out which eye we're using
		GameObject eyeControl = GameObject.Find("EyeControlObject");
		eyeToggle = eyeControl.GetComponent<EyeToggle> ();

		polarityOn = 0;
		
	}
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetInt ("_polarityBool", polarityOn);
		Graphics.Blit (source, destination, material);
	}
	
	void Update(){
		
		currEye = eyeToggle.eye;
		
		if ((currEye == thisEye) || (currEye == 0)) {
			if(Input.GetKeyDown("0")){
				if(polarityOn == 1){
					polarityOn = 0;
				}
				else{
					polarityOn = 1;
				}

			}

		}
	}
}
