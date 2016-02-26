using UnityEngine;
using System.Collections;

public static class LibCVToUnityConversion {

	public static Vector3 LibCVToUnity(Vector3 inVec) {
		Vector3 outVec = inVec;
		outVec.y = -outVec.y;
		return outVec;
	}

	public static Quaternion LibCVToUnity(Quaternion inQuat) {
		Quaternion outQuat = inQuat;
		outQuat.y = -outQuat.y;
		return outQuat;
	}
}
