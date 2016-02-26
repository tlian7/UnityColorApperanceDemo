using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class StreamingRGBD : MonoBehaviour {
	const string PluginName = "LibCVPlugin";

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern StreamingRGBDSensorState GetRGBDSensorState(string url);

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern uint GetRGBDUpdateCount(string url);
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void SetRGBDSensorTextureFromUnity(string url, System.IntPtr texture);
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void GetDepthAndXYZFromRGBDSensor(string url, [In, Out] byte[] valid_depths, 
	                                          [In, Out] float[] x, [In, Out] float[] y, [In, Out] float[] z);
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void InitializeRGBDSensor(string url);

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern int ShutdownRGBDSensor(string url);

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport(PluginName)]
	#endif
	private static extern IntPtr GetRenderEventFunc();

	static SortedList<string, Texture2D> urlsToTextures = new SortedList<string, Texture2D>();
	static SortedList<string, StreamingRGBDSensorState> urlsToTextureStates = new SortedList<string, StreamingRGBDSensorState>();

	StreamingRGBDSensorState stateAtAssignment = new StreamingRGBDSensorState();
	Texture2D assignedTexture = null;
	uint updateCount = 0;
	Material material;

	public string URL;
	public bool isLinear = true;
	public Material defaultMaterial;
	int downsampleFactor = 4;

	void OnEnable() {
		stateAtAssignment.isConnected = false;
		LibCVSingleton.InitializeLibCV ();
		InitializeRGBDSensor (URL);
	}

	void OnDisable() {
		ShutdownRGBDSensor (URL);
	}

	// Use this for initialization
	IEnumerator Start () {
		material = Instantiate (defaultMaterial);
		yield return StartCoroutine("CallPluginAtEndOfFrames");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool TextureStatesMatch(StreamingRGBDSensorState a, StreamingRGBDSensorState b)
	{
		return (a.widthRGB == b.widthRGB
				&& a.heightRGB == b.heightRGB
		        && a.isConnected == b.isConnected);
	}

	bool DepthStatesMatch(StreamingRGBDSensorState a, StreamingRGBDSensorState b)
	{
		return (a.widthDepth == b.widthDepth
		        && a.heightDepth == b.heightDepth
		        && a.isConnected == b.isConnected);
	}

	void PrintTextureState(StreamingRGBDSensorState state)
	{
		Debug.Log ("WidthRGB: " + state.widthRGB);
		Debug.Log ("HeightRGB: " + state.heightRGB);
		Debug.Log ("WidthDepth: " + state.widthDepth);
		Debug.Log ("HeightDepth: " + state.heightDepth);
		Debug.Log ("IsConnected: " + state.isConnected);
	}

	byte[] validDepth = null;
	float[] x_pos;
	float[] y_pos;
	float[] z_pos;
	GameObject rgbdMeshParent = null;

	void updateRGBDMesh(StreamingRGBDSensorState curState, StreamingRGBDSensorState oldState)
	{
		//Check if we actually need to update the mesh
		uint curCount = GetRGBDUpdateCount(URL);
		if (updateCount == curCount)
			return;

		updateCount = curCount;

		//Handle resolution changes / initial allocation
		int width = curState.widthDepth;
		int height = curState.heightDepth;
		if (!DepthStatesMatch (curState, oldState) 
		    || validDepth == null 
		    || x_pos == null 
		    || y_pos == null 
		    || z_pos == null) {
			int numElems = width*height;
			validDepth = new byte[numElems];
			x_pos = new float[numElems];
			y_pos = new float[numElems];
			z_pos = new float[numElems];

			PatchesPerImageX = width / 20;
			PatchesPerImageY = height / 15;
			CreatePatches ();
		}

		//Step 1. Call plugin to populate fields accordingly
		GetDepthAndXYZFromRGBDSensor (URL, validDepth, x_pos, y_pos, z_pos);

		//Step 2. Update the 
		UpdatePatches (width, height);
	}

	List<GameObject> patches = new List<GameObject>();
	int PatchesPerImageX = 8;
	int PatchesPerImageY = 8;

	//Meshes are broken up into a variety of patches for fast rendering
	public void CreatePatches()
	{
		//Create the mesh parent if needed
		if (rgbdMeshParent == null) {
			rgbdMeshParent = new GameObject();
			rgbdMeshParent.transform.parent = this.transform;
			rgbdMeshParent.transform.localPosition = Vector3.zero;
			rgbdMeshParent.transform.localRotation = Quaternion.identity;
		}

		DestroyPatches ();

		for(int i = 0; i < PatchesPerImageY; i++) {
			for(int j = 0; j < PatchesPerImageX; j++) {
				GameObject patch = StreamingRGBDMeshHelper.CreateRGBDGeometryPatch(material);
				patch.transform.SetParent (rgbdMeshParent.transform);
				patch.transform.localPosition = Vector3.zero;
				patch.transform.localRotation = Quaternion.identity;
				patches.Add(patch);
			}
		}
	}

	//Assumed that UpdatePointCloud has already been called
	public void UpdatePatches(int width, int height)
	{
		int patchWidth = width / PatchesPerImageX;
		int patchHeight = height / PatchesPerImageY;
		
		for(int y = 0; y < PatchesPerImageY; y++) {
			for(int x = 0; x < PatchesPerImageX; x++) {
				int patchIdx = x + y * PatchesPerImageX;
				int xStart = x * patchWidth;
				int yStart = y * patchHeight;
				StreamingRGBDMeshHelper.UpdateRGBDGeometryPatch(patches[patchIdx],
				                                           ref x_pos, ref y_pos, ref z_pos,
				                                           ref validDepth, width, height,
				                                                xStart, yStart, patchWidth, patchHeight, downsampleFactor, downsampleFactor);
			}
		}
	}
	
	public void DestroyPatches()
	{
		while (patches.Count > 0) {
			int lastIdx = patches.Count - 1;
			GameObject obj = patches[lastIdx];
			Destroy(obj);
			patches.RemoveAt(lastIdx);
		}
	}

	private IEnumerator CallPluginAtEndOfFrames()
	{
		while (true) {
			// Wait until all frame rendering is done
			yield return new WaitForEndOfFrame();
			//continue;

			StreamingRGBDSensorState curState = GetRGBDSensorState(URL);
			if(!curState.isConnected)
				continue;

			//PrintTextureState(curState);

			StreamingRGBDSensorState oldState = new StreamingRGBDSensorState();
			if(urlsToTextureStates.ContainsKey(URL))
			{
				oldState = urlsToTextureStates[URL];
			}
			
			//PrintTextureState(oldState);
			updateRGBDMesh(curState, oldState);

			if(!TextureStatesMatch(curState, oldState))
			{
				Texture2D texture = null;
				if(urlsToTextures.ContainsKey(URL))
				{
					texture = urlsToTextures[URL];
				}

				if(texture != null)
				{
					Destroy(texture);
				}
				TextureFormat format = TextureFormat.RGB24;
				texture = new Texture2D(curState.widthRGB, curState.heightRGB, format, false, isLinear);
				SetRGBDSensorTextureFromUnity(URL, texture.GetNativeTexturePtr());
				//PrintTextureState(curState);
				if(!urlsToTextures.ContainsKey(URL))
				{
					urlsToTextures.Add(URL, texture);
				}
				else
				{
					urlsToTextures[URL] = texture;
				}
			}

			if(!TextureStatesMatch(stateAtAssignment, curState) && urlsToTextures.ContainsKey(URL))
			{
				stateAtAssignment = curState;
				assignedTexture = urlsToTextures[URL];
				material.SetTexture("_MainTex", assignedTexture);
			}

			if(!urlsToTextureStates.ContainsKey(URL))
			{
				urlsToTextureStates.Add(URL, curState);
			}
			else
			{
				urlsToTextureStates[URL] = curState;
			}
			
			// Issue a plugin event with arbitrary integer identifier.
			// The plugin can distinguish between different
			// things it needs to do based on this ID.
			// For our simple plugin, it does not matter which ID we pass here.
			//GL.IssuePluginEvent(GetRenderEventFunc(), 1);
		}
	}
}
