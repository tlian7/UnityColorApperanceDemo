using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class SecondSightSingleton : MonoBehaviour {
	
	static GameObject SecondSightInstance = null;
	
	const string PluginName = "SecondSightPlugin";
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport(PluginName)]
	#endif
	private static extern void Initialize();

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport(PluginName)]
	#endif
	private static extern void Shutdown();

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport(PluginName)]
	#endif
	private static extern void SetCommand(int command);
	
	public static void InitializeSecondSight() {
		if (SecondSightInstance == null) {
			SecondSightInstance = new GameObject("SecondSight");
			SecondSightInstance.AddComponent<SecondSightSingleton>();
		}
	}


	
	void OnEnable() {
		if (this.gameObject == SecondSightInstance) {
			Initialize();
		}
	}
	
	void OnDisable() {
		if (this.gameObject == SecondSightInstance) {
			Shutdown();
			SecondSightInstance = null;
			Destroy(this.gameObject);
		}
	}

	public static void SetCommandFromInt (int value) {
		InitializeSecondSight ();
		Debug.Log (value);
		SetCommand (value);
	}

	public static void SetCommand(UnityCommandEnum command) {
		InitializeSecondSight ();
		Debug.Log ((int)command);
		SetCommand ((int)command);
	}
	
	
	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
