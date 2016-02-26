using UnityEngine;
using System.Collections;

public class PointerController : MonoBehaviour {

	private float left = -2.1876f;
	private float right = -0.14f;

	private GameObject EyeControlObject;
	private GlobalController globalController;

	public bool showAnswer;

	// Use this for initialization
	void Start () {

		// Keep the pointer invisible
		//gameObject.SetActive (false);

		EyeControlObject = GameObject.Find("EyeControlObject");
		globalController = EyeControlObject.GetComponent<GlobalController> ();

		showAnswer = false;

	}
	
	// Update is called once per frame
	void Update () {
		print ("Updating pointer");

		if (Input.GetKeyDown (KeyCode.R)) {
			print ("Key R was pressed!");
		}

		// Move pointer depending on what is forward
		if (globalController.leftCupisFront) {
			gameObject.transform.position = new Vector3(left,gameObject.transform.position.y,gameObject.transform.position.z);
		} else {
			gameObject.transform.position = new Vector3(right,gameObject.transform.position.y,gameObject.transform.position.z);
		}

		// If R has been pressed reveal answer
//		if (Input.GetKeyDown (KeyCode.R) && !showAnswer) {
//			gameObject.SetActive (true);
//			showAnswer = true;
//		} else if (Input.GetKeyDown (KeyCode.R) && showAnswer){
//			gameObject.SetActive (false);
//			showAnswer = false;
//		}

	
	}
}
