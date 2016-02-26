using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class StreamingRGBDMeshHelper
{
	static List<int> indicesList = new List<int>();
	
	const float maxConnectiveZDiffZNear = 1.0e-1f; //In meters
	const float maxConnectiveZDiffZFar = 2.0e-1f; //In meters
	const float ZFarBegin = 1.0f; //In meters
	const float ZFarEnd = 2.0f; //In meters

	static int[] computeTriangles(ref float[] z_pos, ref byte[] validDepths, int width, int height, int xStart, int yStart, int xPatchSize, int yPatchSize, int xDownSampleFactor, int yDownSampleFactor)
	{
		indicesList.Clear ();

		for (int y = 0; y < yPatchSize-yDownSampleFactor; y+= yDownSampleFactor) {
			for(int x = 0; x < xPatchSize-xDownSampleFactor; x+= xDownSampleFactor) {
				int xDepthMap = x + xStart;
				int yDepthMap = y + yStart;
				xDepthMap = Math.Min(xDepthMap, width-1-xDownSampleFactor);// -2);
				yDepthMap = Math.Min(yDepthMap, height-1-yDownSampleFactor);//-2);
				//Compute image strides
				int strideUL = xDepthMap + yDepthMap * width;
				int strideUR = strideUL + xDownSampleFactor;
				int strideLL = strideUL + width * yDownSampleFactor;
				int strideLR = strideLL + xDownSampleFactor;
				
				//Compute patch strides 
				int strideUL_P = x + y * xPatchSize;
				int strideUR_P = strideUL_P + xDownSampleFactor;
				int strideLL_P = strideUL_P + xPatchSize * yDownSampleFactor;
				int strideLR_P = strideLL_P + xDownSampleFactor;
				
				bool validUL = (validDepths[strideUL] > 0);
				bool validUR = (validDepths[strideUR] > 0);
				bool validLL = (validDepths[strideLL] > 0);
				bool validLR = (validDepths[strideLR] > 0);
				
				//Check if all points in depth map are valid. If so, add a quad composed of two triangles
				if(validUL && validUR && validLL && validLR) {

					float avgZ = 0.25f * (z_pos[strideUL] + z_pos[strideUR] + z_pos[strideLL] + z_pos[strideLR]);
					
					float zInterp = Mathf.Min(1.0f, Mathf.Max(0.0f, (avgZ - ZFarBegin) / (ZFarEnd - ZFarBegin)));
					float maxConnectiveZDiff = maxConnectiveZDiffZNear * (1.0f - zInterp) + zInterp * maxConnectiveZDiffZFar;

					float avgAbsDistA = (Mathf.Abs(z_pos[strideUR]-z_pos[strideUL]) + Mathf.Abs(z_pos[strideUR]-z_pos[strideLL]) + Mathf.Abs(z_pos[strideLL]-z_pos[strideUL]))/3.0f;
					
					if(avgAbsDistA < maxConnectiveZDiff) {
						indicesList.Add(strideUR_P);
						indicesList.Add(strideUL_P);
						indicesList.Add(strideLL_P);

						indicesList.Add(strideLL_P);
						indicesList.Add(strideUL_P);
						indicesList.Add(strideUR_P);
					}
					
					float avgAbsDistB = (Mathf.Abs(z_pos[strideUR]-z_pos[strideLR]) + Mathf.Abs(z_pos[strideUR]-z_pos[strideLL]) + Mathf.Abs(z_pos[strideLL]-z_pos[strideLR]))/3.0f;
					if(avgAbsDistB < maxConnectiveZDiff) {
						indicesList.Add(strideLL_P);
						indicesList.Add(strideLR_P);
						indicesList.Add(strideUR_P);

						indicesList.Add(strideUR_P);
						indicesList.Add(strideLR_P);
						indicesList.Add(strideLL_P);
					}
				}
			}
		}
		return indicesList.ToArray ();
	}
	
	public static GameObject CreateRGBDGeometryPatch(Material material)
	{
		GameObject gameObj = new GameObject ();
		Mesh mesh = new Mesh();
		MeshFilter meshFilter = gameObj.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
		MeshRenderer meshRenderer = gameObj.AddComponent<MeshRenderer>();
		meshRenderer.material = material;
		return gameObj;
	}
	
	public static void UpdateRGBDGeometryPatch(GameObject patch, ref float[] x_pos, ref float[] y_pos, ref float[] z_pos, ref byte[] validDepths, int width, int height, int xStart, int yStart, int xPatchSize, int yPatchSize, int xDownSampleFactor, int yDownSampleFactor)
	{
		xPatchSize += xDownSampleFactor;
		yPatchSize += yDownSampleFactor;
		Mesh mesh = patch.GetComponent<MeshFilter> ().mesh;
		int vertexCount = xPatchSize * yPatchSize;
		Vector3[] verts = new Vector3[vertexCount];
		Vector2[] uvs = new Vector2[vertexCount];
		
		int xEnd = Math.Min (xStart + xPatchSize, width);
		int yEnd = Math.Min (yStart + yPatchSize, height);
		for (int y = yStart; y < yEnd; y+= yDownSampleFactor) {
			for (int x = xStart; x < xEnd; x+= xDownSampleFactor) {
				int strideP = (x - xStart) + (y - yStart) * xPatchSize;
				int stride = x + y * width;
				verts[strideP] = LibCVToUnityConversion.LibCVToUnity(new Vector3(x_pos[stride], y_pos[stride], z_pos[stride]));
				//verts[strideP] = new Vector3(x_pos[stride], -y_pos[stride], z_pos[stride]);
				uvs[strideP] = new Vector2(((float)x+0.5f) / (float)width, ((float)y+0.5f) / (float)height);
			}
		}
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = computeTriangles (ref z_pos, ref validDepths, width, height, xStart, yStart, xPatchSize, yPatchSize, xDownSampleFactor, yDownSampleFactor);
		mesh.RecalculateBounds ();
	}
}