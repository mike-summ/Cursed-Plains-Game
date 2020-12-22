using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour {

	WorldInfo wi;

	void Start () {
		wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo> ();

		if (wi.princeKey) {
			gameObject.SetActive (false);
		}
	}

	public void openGate () {
		wi.princeKey = true;

		gameObject.SetActive (false);
		Dialogue d = GameObject.FindGameObjectWithTag ("Message").GetComponent<Dialogue> ();
		string[] start = new string[] {"Kill the final Boss", "To finally win."};
		d.StartDialogue(start);
	}
}
