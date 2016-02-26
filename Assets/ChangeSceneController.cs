using UnityEngine;
using System.Collections;

public class ChangeSceneController : MonoBehaviour
{

	public KeyCode changeScene = KeyCode.Escape;

	public enum Scene
	{
		Room,
		Depth}
	;
	public Scene currScene;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Escape Application
		if (Input.GetKeyDown (changeScene))
		if (currScene == Scene.Room) {
			Application.LoadLevel ("DepthScene");
		} else if (currScene == Scene.Depth) {
			Application.LoadLevel ("RoomScene");
		}
	}
}
