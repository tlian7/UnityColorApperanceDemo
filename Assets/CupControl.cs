using UnityEngine;
using System.Collections;

public class CupControl : MonoBehaviour
{

	private GlobalController gc;
	private Transform forward;
	private Transform backward;

	private GameObject cupLeftFront;
	private GameObject cupRightFront;

	private GameObject cupLeftBehind;
	private GameObject cupRightBehind;


	// Use this for initialization
	void Start ()
	{

		//Used to determine which cup configuration we want
		GameObject eyeControl = GameObject.Find ("EyeControlObject");
		gc = eyeControl.GetComponent<GlobalController> ();

		//Find cups
		cupLeftFront = GameObject.Find ("cupLeftFront");
		cupRightFront = GameObject.Find ("cupRightFront");
		cupLeftBehind = GameObject.Find ("cupLeftBehind");
		cupRightBehind = GameObject.Find ("cupRightBehind");

		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gc.leftCupisFront) {
			cupLeftFront.SetActive(true);
			cupRightBehind.SetActive(true);

			cupLeftBehind.SetActive(false);
			cupRightFront.SetActive(false);

		} else {

			cupLeftFront.SetActive(false);
			cupRightBehind.SetActive(false);
			
			cupLeftBehind.SetActive(true);
			cupRightFront.SetActive(true);

		}

	
	}
}
