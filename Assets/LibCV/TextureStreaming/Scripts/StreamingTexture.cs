using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class StreamingTexture : MonoBehaviour {
	const string PluginName = "LibCVPlugin";

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern StreamingTextureState GetTextureState(string url);
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void SetTextureFromUnity(string url, System.IntPtr texture);
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void InitializeTexture(string url);
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern int ShutdownTexture(string url);

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport(PluginName)]
	#endif
	private static extern IntPtr GetRenderEventFunc();

	static SortedList<string, Texture2D> urlsToTextures = new SortedList<string, Texture2D>();
	static SortedList<string, StreamingTextureState> urlsToTextureStates = new SortedList<string, StreamingTextureState>();

	StreamingTextureState stateAtAssignment = new StreamingTextureState();
	Texture2D assignedTexture = null;

	public string URL;
	public bool isLinear = true;
	public Renderer[] renderersToUpdate;

	//Note - Only a subset of texture formats are supported by Unity
	TextureFormat GetTextureFormatFromState(StreamingTextureState state, ref bool wasSuccess) {
		TextureFormat format = new TextureFormat();
		SerializedMatrix_SerializedMatrixType type = (SerializedMatrix_SerializedMatrixType)state.data_type;
		switch (type) {
		case SerializedMatrix_SerializedMatrixType.SerializedMatrix_SerializedMatrixType_SMT_UNSIGNED_BYTE:
			switch(state.num_channels) {
			case 1:
				format = TextureFormat.Alpha8;
				wasSuccess = true;
				break;
			case 2:
				//Unsupported
				break;
			case 3:
				format = TextureFormat.RGB24;
				wasSuccess = true;
				break;
			case 4:
				format = TextureFormat.RGBA32;
				wasSuccess = true;
				break;
			}
			break;
		case SerializedMatrix_SerializedMatrixType.SerializedMatrix_SerializedMatrixType_SMT_SIGNED_BYTE:
			switch(state.num_channels) {
			case 1:
				format = TextureFormat.Alpha8;
				wasSuccess = true;
				break;
			case 2:
				//Unsupported
				break;
			case 3:
				format = TextureFormat.RGB24;
				wasSuccess = true;
				break;
			case 4:
				format = TextureFormat.RGBA32;
				wasSuccess = true;
				break;
			}
			break;
		case SerializedMatrix_SerializedMatrixType.SerializedMatrix_SerializedMatrixType_SMT_UNSIGNED_SHORT:
			switch(state.num_channels) {
			case 1:
				format = TextureFormat.RHalf;
				wasSuccess = true;
				break;
			case 2:
				format = TextureFormat.RGHalf;
				wasSuccess = true;
				break;
			case 3:
				//Unsupported
				break;
			case 4:
				format = TextureFormat.RGBAHalf;
				wasSuccess = true;
				break;
			}
			break;
		case SerializedMatrix_SerializedMatrixType.SerializedMatrix_SerializedMatrixType_SMT_SIGNED_SHORT:
			switch(state.num_channels) {
			case 1:
				format = TextureFormat.RHalf;
				wasSuccess = true;
				break;
			case 2:
				format = TextureFormat.RGHalf;
				wasSuccess = true;
				break;
			case 3:
				//Unsupported
				break;
			case 4:
				format = TextureFormat.RGBAHalf;
				wasSuccess = true;
				break;
			}
		break;
		case SerializedMatrix_SerializedMatrixType.SerializedMatrix_SerializedMatrixType_SMT_FLOAT:
			switch(state.num_channels) {
			case 1:
				format = TextureFormat.RFloat;
				wasSuccess = true;
				break;
			case 2:
				format = TextureFormat.RGFloat;
				wasSuccess = true;
				break;
			case 3:
				//Unsupported
				break;
			case 4:
				format = TextureFormat.RGBAFloat;
				wasSuccess = true;
				break;
			}
			break;
		}
		return format;
	}

	void OnEnable() {
		stateAtAssignment.isConnected = false;
		LibCVSingleton.InitializeLibCV ();
		InitializeTexture (URL);
	}

	void OnDisable() {
		ShutdownTexture (URL);
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return StartCoroutine("CallPluginAtEndOfFrames");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool TextureStatesMatch(StreamingTextureState a, StreamingTextureState b)
	{
		return (a.width == b.width
			&& a.height == b.height
			&& a.num_channels == b.num_channels 
			&& a.data_type == b.data_type
			&& a.isConnected == b.isConnected);
	}

	void PrintTextureState(StreamingTextureState state)
	{
		Debug.Log ("Width: " + state.width);
		Debug.Log ("Height: " + state.height);
		Debug.Log ("NumChannels: " + state.num_channels);
		Debug.Log ("IsConnected: " + state.isConnected);
		Debug.Log ("NumTimesUpdated: " + state.numTimesUpdated);
	}

	private IEnumerator CallPluginAtEndOfFrames()
	{
		while (true) {
			// Wait until all frame rendering is done
			yield return new WaitForEndOfFrame();
			//continue;

			StreamingTextureState curState = GetTextureState(URL);
			if(!curState.isConnected)
				continue;

			//PrintTextureState(curState);

			StreamingTextureState oldState = new StreamingTextureState();
			if(urlsToTextureStates.ContainsKey(URL))
			{
				oldState = urlsToTextureStates[URL];
			}

			if(!TextureStatesMatch(curState, oldState))
			{
				Debug.Log("There was a change!");
				Texture2D texture = null;
				if(urlsToTextures.ContainsKey(URL))
				{
					texture = urlsToTextures[URL];
				}

				if(texture != null)
				{
					Destroy(texture);
				}
				bool wasSuccess = false;
				TextureFormat format = GetTextureFormatFromState(curState, ref wasSuccess);
				if(wasSuccess)
				{
					texture = new Texture2D(curState.width, curState.height, format, false, isLinear);
					SetTextureFromUnity(URL, texture.GetNativeTexturePtr());
					Debug.Log("I got called!");
					Debug.Log("The state is:");
					PrintTextureState(curState);
					if(!urlsToTextures.ContainsKey(URL))
					{
						urlsToTextures.Add(URL, texture);
					}
					else
					{
						urlsToTextures[URL] = texture;
					}
				}
			}

			if(!TextureStatesMatch(stateAtAssignment, curState) && urlsToTextures.ContainsKey(URL))
			{
				stateAtAssignment = curState;
				assignedTexture = urlsToTextures[URL];
				Debug.Log("Assigning to textures!");
				foreach(Renderer renderer in renderersToUpdate)
				{
					renderer.material.SetTexture("_MainTex", assignedTexture);
				}
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
