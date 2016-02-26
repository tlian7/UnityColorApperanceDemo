using UnityEngine;
using System.Collections;

public class BackgroundColor : MonoBehaviour {

	// This is true if it is the left camera
	public bool cameraLeft = false;
	public bool backgroundPolarityOn = false;
	private GameObject BackgroundPlane;

	// Use this for initialization
	void Start () {
		BackgroundPlane = GameObject.Find ("BackgroundPlane");
	
	}

	public void OnPreRender ()
	{
		if (backgroundPolarityOn) {
			if(cameraLeft){
				BackgroundPlane.GetComponent<Renderer> ().material.color = new Color(0.0f,0.0f,0.0f);
			}else{
				BackgroundPlane.GetComponent<Renderer> ().material.color = new Color(1.0f,1.0f,1.0f);
			}
		
		} else {
			BackgroundPlane.GetComponent<Renderer> ().material.color = new Color(0.83f,0.83f,0.83f);
			
			
		}
	}

	
	// Update is called once per frame
	void Update () {
		// Turn on the switched polarity background
		if (Input.GetKeyDown (KeyCode.P)) {
			backgroundPolarityOn = !backgroundPolarityOn;
		}
	
	}
}
