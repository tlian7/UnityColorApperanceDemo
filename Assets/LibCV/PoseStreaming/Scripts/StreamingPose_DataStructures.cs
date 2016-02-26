using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//This structure must match the structure defined in RGBDStreamingPlugin for interop code to work properly
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)] //Fancy way of telling Unity to layout this data-structure in memory precisely how we define it here
public struct StreamingPoseState
{
	public bool isConnected;
	public float p_x;
	public float p_y;
	public float p_z;
	public float q_x;
	public float q_y;
	public float q_z;
	public float q_w;
};