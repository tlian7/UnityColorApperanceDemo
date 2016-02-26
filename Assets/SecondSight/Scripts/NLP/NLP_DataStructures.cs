using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//This structure must match the structure defined in StreamingTexturePlugin for interop code to work properly
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)] //Fancy way of telling Unity to layout this data-structure in memory precisely how we define it here
public struct UnityNLPState
{
	public int index;
	public bool isHighlighted;
};