using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoDisplayController : MonoBehaviour {

	private bool displayInfo;
	private CanvasGroup CG;
	// Use this for initialization
	void Start () {
		displayInfo = false;
		CG = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)) {
			displayInfo = !displayInfo;
		}

		if (!displayInfo) {
			while(CG.alpha > 0){
				CG.alpha -= Time.deltaTime*0.0f;
			}
		} else {
			while(CG.alpha < 1){
			 	CG.alpha += Time.deltaTime*0.0f;;
			}
		}
	}


}
