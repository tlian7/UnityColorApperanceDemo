using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class StreamingPose : MonoBehaviour {
	const string PluginName = "LibCVPlugin";

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern StreamingPoseState GetPoseState(string url);

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void InitializePose(string url);
		
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void ShutdownPose(string url);

	StreamingPoseState poseState;

	public string URL;
	public Transform pose;

	void OnEnable() {
		poseState.isConnected = false;
		InitializePose (URL);
	}

	void OnDisable() {
		ShutdownPose (URL);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (pose != null) {
			poseState = GetPoseState (URL);
			if (poseState.isConnected) {
				Vector4 qVec = new Vector4(poseState.q_x, poseState.q_y, poseState.q_z, poseState.q_w);
				if(qVec.magnitude > 0.0f && !float.IsInfinity(qVec.magnitude) && !float.IsNaN(qVec.magnitude)) {
					Vector3 t = new Vector3(poseState.p_x, poseState.p_y, poseState.p_z);
					Quaternion q = new Quaternion(poseState.q_x, poseState.q_y, poseState.q_z, poseState.q_w);
					pose.localPosition = LibCVToUnityConversion.LibCVToUnity(t);
					pose.localRotation = LibCVToUnityConversion.LibCVToUnity(q);
					/*
					pose.localPosition = new Vector3(poseState.p_x, -poseState.p_y, poseState.p_z);
					pose.localRotation = new Quaternion(poseState.q_x, -poseState.q_y, poseState.q_z, poseState.q_w);
					*/
				}
			}
		}
	}
}
