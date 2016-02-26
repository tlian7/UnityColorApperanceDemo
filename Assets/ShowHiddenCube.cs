using UnityEngine;
using System.Collections;

// This cannot go on hidden cube because an inactive object cannot turn itself back on?
public class ShowHiddenCube : MonoBehaviour
{

	private GameObject hiddenCube;
	private bool showObject = false;

	// Use this for initialization
	void Start ()
	{
		hiddenCube = GameObject.Find ("HiddenCube");
		hiddenCube.SetActive (showObject);
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.P)) {
			showObject = !showObject;
			hiddenCube.SetActive (showObject);
		}
	
	}
}
