using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class LibCVSingleton : MonoBehaviour {

	static GameObject LibCVInstance = null;

	const string PluginName = "LibCVPlugin";

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport(PluginName)]
	#endif
	private static extern IntPtr GetRenderEventFunc();

	public static void InitializeLibCV() {
		if (LibCVInstance == null) {
			LibCVInstance = new GameObject("LibCV");
			LibCVInstance.AddComponent<LibCVSingleton>();
		}
	}

	void OnEnable() {
	}
	
	void OnDisable() {
		if (this.gameObject == LibCVInstance) {
			LibCVInstance = null;
			Destroy(this.gameObject);
		}
	}


	// Use this for initialization
	IEnumerator Start () {
		yield return StartCoroutine("CallPluginAtEndOfFrames");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator CallPluginAtEndOfFrames()
	{
		while (true) {
			// Wait until all frame rendering is done
			yield return new WaitForEndOfFrame ();
//			GL.IssuePluginEvent(GetRenderEventFunc(), 1);
		}
	}
}
