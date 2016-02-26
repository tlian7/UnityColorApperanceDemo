using UnityEngine;
using System.Collections;

public class EyeToggle : MonoBehaviour {

	public int eye;
	// 0 is both eyes
	// 1 is left eye
	// 2 is right eye
	
	// Use this for initialization
	void Start () {
		eye = 0;;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("z")) {
			eye = 0;
		} else if (Input.GetKeyDown ("x")) {
			eye = 1;
		} else if (Input.GetKeyDown ("c")) {
			eye = 2;
		}

	}
}
