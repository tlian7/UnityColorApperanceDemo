using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Colorblind")]
public class ColorblindFilter : ImageEffectBase {

	private Matrix4x4 cbMatrix;
	public Text colorblindText;

	public int thisEye = 1; //Set which eye this filter is placed at
	public int currEye;
	private EyeToggle eyeToggle;

	private int delta = 1;

	// Keep track of which color anomalous peak we are using
	public int anomCase = 1;

	void Start(){
		cbMatrix = Matrix4x4.identity;
		colorblindText.text = "Normal";

		//Find out which eye we're using
		GameObject eyeControl = GameObject.Find("EyeControlObject");
		eyeToggle = eyeControl.GetComponent<EyeToggle> ();

	}
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetMatrix ("_cbMatrix",cbMatrix);
		Graphics.Blit (source, destination, material);
	}

	void Update(){

		currEye = eyeToggle.eye;

		if ((currEye == thisEye) || (currEye == 0)) {
			if (Input.GetKeyDown ("1")) {
				anomCase = 0;
				cbMatrix = Matrix4x4.identity;
				colorblindText.text = "Normal";
			}
			if (Input.GetKeyDown ("2")) {
				anomCase = 0;
				cbMatrix = Matrix4x4.identity;
				cbMatrix [0, 0] = 0.0f;
				cbMatrix [1, 0] = 1.2372f;
				cbMatrix [2, 0] = -0.2568f;
				colorblindText.text = "Protanope";
			}
			if (Input.GetKeyDown ("3")) {
				anomCase = 0;
				cbMatrix = Matrix4x4.identity;
				cbMatrix [0, 1] = 0.7746f;
				cbMatrix [1, 1] = 0.0f;
				cbMatrix [2, 1] = 0.2782f;
				colorblindText.text = "Deuteranope";
			}
			if (Input.GetKeyDown ("4")) {
				anomCase = 0;
				cbMatrix = Matrix4x4.identity;
				cbMatrix [0, 2] = -0.8025f;
				cbMatrix [1, 2] = 1.4057f;
				cbMatrix [2, 2] = 0.0f;
				colorblindText.text = "Tritanope";
			}

			if(Input.GetKeyDown("5")){

				anomCase = anomCase+delta;

				if(anomCase  >= 9){
					delta = -1;
				}

				if(anomCase <= 1){
					delta = 1;
				}

				switch (anomCase)
				{
				case 1:
					cbMatrix = Matrix4x4.identity;
					colorblindText.text = "Normal";
					break;
				case 2:
					cbMatrix[0,0] = 1.1839f;
					cbMatrix[0,1] = 0.0791f;
					cbMatrix[0,2] = 0.0049f;
					cbMatrix[1,0] = -0.2128f;
					cbMatrix[1,1] = 0.9021f;
					cbMatrix[1,2] = -0.0058f;
					cbMatrix[2,0] = 0.0182f;
					cbMatrix[2,1] = 0.0190f;
					cbMatrix[2,2] = 1.0007f;
					colorblindText.text = "Anomalous: 563 nm";
					break;

				case 3:
					cbMatrix[0,0] = 1.2206f;
					cbMatrix[0,1] = 0.0953f;
					cbMatrix[0,2] = 0.0062f;
					cbMatrix[1,0] = -0.2536f;
					cbMatrix[1,1] = 0.8834f;
					cbMatrix[1,2] = -0.0071f;
					cbMatrix[2,0] = 0.0189f;
					cbMatrix[2,1] = 0.0206f;
					cbMatrix[2,2] = 1.0006f;
					colorblindText.text = "Anomalous: 556 nm";
					break;

				case 4:
					cbMatrix[0,0] = 1.2585f;
					cbMatrix[0,1] = 0.1155f;
					cbMatrix[0,2] = 0.0076f;
					cbMatrix[1,0] = -0.2947f;
					cbMatrix[1,1] = 0.8602f;
					cbMatrix[1,2] = -0.0086f;
					cbMatrix[2,0] = 0.0176f;
					cbMatrix[2,1] = 0.0223f;
					cbMatrix[2,2] = 1.0004f;
					colorblindText.text = "Anomalous: 550 nm";
					break;

				case 5:
					cbMatrix[0,0] = 1.2935f;
					cbMatrix[0,1] = 0.1383f;
					cbMatrix[0,2] = 0.0090f;
					cbMatrix[1,0] = -0.3306f;
					cbMatrix[1,1] = 0.8342f;
					cbMatrix[1,2] = -0.0100f;
					cbMatrix[2,0] = 0.0129f;
					cbMatrix[2,1] = 0.0238f;
					cbMatrix[2,2] = 1.0000f;
					colorblindText.text = "Anomalous: 547 nm";
					break;

				case 6:
					cbMatrix[0,0] = 0.8149f;
					cbMatrix[0,1] = 0.0612f;
					cbMatrix[0,2] = -0.0027f;
					cbMatrix[1,0] = 0.2425f;
					cbMatrix[1,1] = 0.9249f;
					cbMatrix[1,2] = 0.0037f;
					cbMatrix[2,0] = -0.0684f;
					cbMatrix[2,1] = 0.0137f;
					cbMatrix[2,2] = 0.9988f;
					colorblindText.text = "Anomalous: 545 nm";
					break;

				case 7:					
					cbMatrix[0,0] = 0.4709f;
					cbMatrix[0,1] = 0.0084f;
					cbMatrix[0,2] = -0.0111f;
					cbMatrix[1,0] = 0.6475f;
					cbMatrix[1,1] = 0.9848f;
					cbMatrix[1,2] = 0.0129f;
					cbMatrix[2,0] = -0.1146f;
					cbMatrix[2,1] = 0.0105f;
					cbMatrix[2,2] = 0.9988f;
					colorblindText.text = "Anomalous: 543 nm";
					break;

				case 8:
					cbMatrix[0,0] = 0.2549f;
					cbMatrix[0,1] = -0.0104f;
					cbMatrix[0,2] = -0.0158f;
					cbMatrix[1,0] = 0.9009f;
					cbMatrix[1,1] = 1.0047f;
					cbMatrix[1,2] = 0.0178f;
					cbMatrix[2,0] = -0.1420f;
					cbMatrix[2,1] = 0.0120f;
					cbMatrix[2,2] = 0.9992f;
					colorblindText.text = "Anomalous: 542 nm";
					break;

				case 9:
					cbMatrix[0,0] = 0.0761f;
					cbMatrix[0,1] = -0.0074f;
					cbMatrix[0,2] = -0.0191f;
					cbMatrix[1,0] = 1.1097f;
					cbMatrix[1,1] = 0.9981f;
					cbMatrix[1,2] = 0.0210f;
					cbMatrix[2,0] = -0.1629f;
					cbMatrix[2,1] = 0.0179f;
					cbMatrix[2,2] = 1.0002f;
					colorblindText.text = "Anomalous: 541 nm";
					break;

				default:
					cbMatrix = Matrix4x4.identity;
					break;
				}
			}
		}

	}
}
