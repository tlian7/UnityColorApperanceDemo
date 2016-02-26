using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	private bool showInfo;

	// Use this for initialization
	void Start () {
		showInfo = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			showInfo = !showInfo;
		}


	}
}
