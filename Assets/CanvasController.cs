using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {

	private bool showInfo;
	private CanvasGroup canvasGroup;
	// Use this for initialization
	void Start () {
		showInfo = false;
		canvasGroup = GetComponent<CanvasGroup> ();	
		canvasGroup.alpha = 0.8f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Tab)) {
			showInfo = !showInfo;
		}

		if (showInfo) {
			canvasGroup.alpha = 0.8f;
		} else {
			canvasGroup.alpha = 0;
		}
	
	}
}
