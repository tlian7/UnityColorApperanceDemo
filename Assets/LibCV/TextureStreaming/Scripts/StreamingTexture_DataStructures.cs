using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/*NOTE - It is extremely important that the data type 
 *numbers here match the data type numbers for SerializedMatrix data types defined in
 *libcv. If they do not match exactly, there is a very good chance none of the texture streaming code will work / it may break for certain formats
 */
public enum SerializedMatrix_SerializedMatrixType {
	SerializedMatrix_SerializedMatrixType_SMT_UNSIGNED_BYTE = 0,
	SerializedMatrix_SerializedMatrixType_SMT_SIGNED_BYTE = 1,
	SerializedMatrix_SerializedMatrixType_SMT_UNSIGNED_SHORT = 2,
	SerializedMatrix_SerializedMatrixType_SMT_SIGNED_SHORT = 3,
	SerializedMatrix_SerializedMatrixType_SMT_UNSIGNED_INT = 4,
	SerializedMatrix_SerializedMatrixType_SMT_SIGNED_INT = 5,
	SerializedMatrix_SerializedMatrixType_SMT_FLOAT = 6,
	SerializedMatrix_SerializedMatrixType_SMT_DOUBLE = 7
};

//This structure must match the structure defined in StreamingTexturePlugin for interop code to work properly
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)] //Fancy way of telling Unity to layout this data-structure in memory precisely how we define it here
public struct StreamingTextureState
{
	public int width;
	public int height;
	public int num_channels;
	public int data_type;
	public bool isConnected;
	public int numTimesUpdated;
};