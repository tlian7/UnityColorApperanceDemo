using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GlobalController : MonoBehaviour
{

	public bool leftCupisFront;
	private GameObject LeftEye;
	private GameObject RightEye;
	private Camera cameraLeft;
	private Camera cameraRight;
	public bool blankLeft;
	public bool blankRight;
	private float left = -1.297f;
	private float right = -0.774f;

	private GameObject Pointer;
	private bool showAnswer;

	private GameObject Floor;
	private bool showFloor;

	private GameObject CameraRig;
	private OVRManager manager;
	private bool trackingOn = false;
	public Text trackingText;

	public bool binocularRivalryOn = false;


	// Use this for initialization
	void Start ()
	{

		leftCupisFront = false;
		LeftEye = GameObject.Find ("LeftEyeAnchor");
		RightEye = GameObject.Find ("RightEyeAnchor");
		cameraLeft = LeftEye.GetComponent<Camera> ();
		cameraRight = RightEye.GetComponent<Camera> ();

		blankLeft = false;
		blankRight = false;

		Pointer = GameObject.Find ("Pointer");
		showAnswer = false;
		Pointer.SetActive (false);

		Floor = GameObject.Find ("GroundPlane");
		showFloor = false;
		Floor.SetActive (false);

		CameraRig = GameObject.Find("OVRCameraRig");
		manager = CameraRig.GetComponent<OVRManager> ();
		manager.usePositionTracking = false;


	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			cameraLeft.cullingMask = 0;
			cameraRight.cullingMask = 0;
			blankLeft = true;
			blankRight = true;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			cameraLeft.cullingMask = ~(1 << 9);
			cameraRight.cullingMask = ~(1 << 8);;
			blankLeft = false;
			blankRight = false;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow) && (blankLeft == false)) {
			cameraLeft.cullingMask = 0;
			blankLeft = true;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) && (blankLeft == true)) {
			cameraLeft.cullingMask = ~(1 << 9);
			blankLeft = false;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow) && (blankRight == false)) {
			cameraRight.cullingMask = 0;
			blankRight = true;
		} else if (Input.GetKeyDown (KeyCode.RightArrow) && (blankRight == true)) {
			cameraRight.cullingMask = ~(1 << 8);;
			blankRight = false;
		}

		// Press return to randomize cups.
		if (Input.GetKeyDown (KeyCode.Return)) {

			StartCoroutine(BlankScreenforSeconds(1));

			// Select a random number to determine which cup is forward and which cup is backwards
			float rvalue = Random.value;
			if (rvalue > 0.5f) {
				leftCupisFront = true;
			} else {
				leftCupisFront = false;
			}

			// Move pointer depending on what is forward
			if (leftCupisFront) {
				Pointer.transform.position = new Vector3 (left, Pointer.transform.position.y, Pointer.transform.position.z);
			} else {
				Pointer.transform.position = new Vector3 (right, Pointer.transform.position.y, Pointer.transform.position.z);
			}

		}
		
		// If G has been pressed reveal answer
		if (Input.GetKeyDown (KeyCode.G)) {
			showAnswer = !showAnswer;
			if(showAnswer){
				Pointer.SetActive(true);
			}else{
				Pointer.SetActive(false);
			}
		}

		// If M is pressed down show the floor
		if (Input.GetKeyDown (KeyCode.M)) {
			showFloor = !showFloor;
			if(showFloor){
				Floor.SetActive(true);
			}else{
				Floor.SetActive(false);
			}
		}

		// If T is pressed turn on positional tracking
		if (Input.GetKeyDown (KeyCode.T)) {
			trackingOn = !trackingOn;
			manager.usePositionTracking = trackingOn;
			if(trackingOn){
				trackingText.text = "MOTION PARALLAX ON";
			}else{
				trackingText.text = "MOTION PARALLAX OFF";
					}
		}


	
	}

	IEnumerator BlankScreenforSeconds (float delay) {
		cameraLeft.cullingMask = 0;
		cameraRight.cullingMask = 0;

		yield return new WaitForSeconds(delay);

		if (!blankLeft) {
			cameraLeft.cullingMask = ~(1 << 9);
		}
		if (!blankRight) {
			cameraRight.cullingMask = ~(1 << 8);
		}

	}

}
