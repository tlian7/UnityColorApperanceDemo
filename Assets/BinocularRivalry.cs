using UnityEngine;
using System.Collections;

public class BinocularRivalry : MonoBehaviour
{

	// This is true if it is the left camera
	public bool cameraLeft;
	public bool binocularRivalryOn = false;
	private GameObject cupLeftFront;
	private GameObject cupRightFront;
	private GameObject cupLeftBehind;
	private GameObject cupRightBehind;
	private GlobalController gc;

	// Use this for initialization
	void Start ()
	{

		// Find the game objects
		cupLeftFront = GameObject.Find ("cupLeftFront");
		cupRightFront = GameObject.Find ("cupRightFront");
		cupLeftBehind = GameObject.Find ("cupLeftBehind");
		cupRightBehind = GameObject.Find ("cupRightBehind");

		GameObject eyeControl = GameObject.Find ("EyeControlObject");
		gc = eyeControl.GetComponent<GlobalController> ();
		
	}

	public void OnPreRender ()
	{
		if (cameraLeft && binocularRivalryOn) {
			// Change the material for the left objects
			if (gc.leftCupisFront) {
				cupLeftFront.GetComponent<Renderer> ().material.color = Color.red;
				cupRightBehind.GetComponent<Renderer> ().material.color = Color.red;
			} else {
				cupLeftBehind.GetComponent<Renderer> ().material.color = Color.red;
				cupRightFront.GetComponent<Renderer> ().material.color = Color.red;
			}

		} else {
			if (gc.leftCupisFront) {
				cupLeftFront.GetComponent<Renderer> ().material.color = Color.blue;
				cupRightBehind.GetComponent<Renderer> ().material.color = Color.blue;
			} else {

				cupLeftBehind.GetComponent<Renderer> ().material.color = Color.blue;
				cupRightFront.GetComponent<Renderer> ().material.color = Color.blue;
			}


		}
	}
	

	// Update is called once per frame
	void Update ()
	{

		// If 5 is pressed, turn on binocular rivalry
		if (Input.GetKeyDown (KeyCode.B)) {
			binocularRivalryOn = !binocularRivalryOn;
		}
	
	}
}
