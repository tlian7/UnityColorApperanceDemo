using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class NLPObject : MonoBehaviour {

	const string PluginName = "NLPPlugin";

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void Initialize();

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void Shutdown();
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern UnityNLPState GetNLPState();
	
	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	#else
	[DllImport (PluginName)]
	#endif
	private static extern void SetName(int object_index, string name);

	UnityNLPState prevState;

	public int ObjectIndex = 0;
	public string[] Names;

	public Material HighlightMaterial;

	public GameObject ObjectToClone;

	GameObject ObjectToHighlight;

	bool CompareNLPStates(UnityNLPState a, UnityNLPState b)
	{
		return (a.index == b.index && a.isHighlighted == b.isHighlighted);
	}

	// Use this for initialization
	void Start () {
		ObjectToHighlight = Instantiate (ObjectToClone);
		ObjectToHighlight.transform.parent = ObjectToClone.transform.parent;
		ObjectToHighlight.transform.localPosition = ObjectToClone.transform.localPosition;
		ObjectToHighlight.transform.localRotation = ObjectToClone.transform.localRotation;
		ObjectToHighlight.transform.localScale = ObjectToClone.transform.localScale;
		Renderer[] renderers = ObjectToHighlight.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in renderers) {
			r.material = HighlightMaterial;
		}
		ObjectToHighlight.SetActive (false);
	}

	void OnEnable() {
		Initialize ();

		foreach (string name in Names) {
			SetName(ObjectIndex, name);
		}
	}

	void OnDisable() {
		Shutdown ();
	}

	// Update is called once per frame
	void Update () {
		UnityNLPState state = GetNLPState ();
		if (!CompareNLPStates(state, prevState)) {
			if (state.index == ObjectIndex) {
				Debug.Log ("Found " + gameObject.name);
				if(state.isHighlighted) {
					ObjectToHighlight.SetActive (true);
					ObjectToClone.SetActive(false);
				}
				else {
					ObjectToHighlight.SetActive (false);
					ObjectToClone.SetActive(true);
				}
			}
		}
		prevState = state;
	}
}
