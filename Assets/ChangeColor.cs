using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

	public bool changeColor;
	public bool leftEye;

	private Color normalColor = new Color(0.5f,0.5f,0.5f);
	private Color darkColor = new Color(0.0f,0.0f,0.0f);
	private Color brightColor = new Color(1.0f,1.0f,1.0f);

	private Renderer thisRenderer;
	// Use this for initialization
	void Start () {
		changeColor = false;
		thisRenderer = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.P)){
			changeColor = !changeColor;
			if(changeColor && leftEye){
				print ("change to bright");
				thisRenderer.material.SetColor("_Color",brightColor);
			}
			else if(changeColor && !leftEye) {
				print ("change to dark");
				thisRenderer.material.SetColor("_Color",darkColor);
			}else{
				thisRenderer.material.SetColor("_Color",normalColor);
			}

		}
	}
}
