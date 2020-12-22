using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	WorldInfo wi;

	void Start() {
		wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo> ();
	}
	void Update() {
		GetComponent<Text> ().text = wi.time + " Seconds";
	}
}
