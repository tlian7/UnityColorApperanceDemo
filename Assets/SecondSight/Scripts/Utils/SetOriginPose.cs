using UnityEngine;
using System.Collections;

public class SetOriginPose : MonoBehaviour {
	public Transform oculusPose;
	public Transform streamedPose;
	public Transform sensorBase;
	public Transform sensorBaseOther;

	Matrix4x4 oldPose;
	bool isUpdatingPose = false;

	// Use this for initialization
	void Start () {
	
	}

	void HandleInput() {
		if(Input.GetKeyDown(KeyCode.C)) {
			Debug.Log("Calib key pressed!");
			oldPose = streamedPose.localToWorldMatrix;
			SecondSightSingleton.SetCommand(UnityCommandEnum.CALIBRATE_SYSTEM);
			isUpdatingPose = true;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			Debug.Log("Static Geometry Key Pressed!");
			SecondSightSingleton.SetCommand(UnityCommandEnum.COMPUTE_STATIC_GEOMETRY);
		}
	}

	void HandlePoseReplacement() {
		if (!isUpdatingPose) {
			return;
		}

		Matrix4x4 curPose = streamedPose.worldToLocalMatrix;
		Matrix4x4 delta = curPose * oldPose;
		if (delta != Matrix4x4.identity) {
			Debug.Log("Oh snap, we calibrated!");

			Vector3 pos = streamedPose.localPosition;
			Quaternion R = streamedPose.localRotation;
			Quaternion ROculus = oculusPose.localRotation;
			Vector3 posNew = oculusPose.TransformPoint(pos); //Pretty sure this is correct
			Quaternion Rnew = ROculus * Quaternion.Inverse(R);//Quaternion.Inverse(ROculus) * R; //Not sure if this is correct
			sensorBase.localPosition = posNew;
			sensorBase.localRotation = Rnew;
			isUpdatingPose = false;
		} else {
			Debug.Log("Still waiting");
		}
	}

	// Update is called once per frame
	void Update () {
		HandleInput ();
		HandlePoseReplacement ();

	}
}
