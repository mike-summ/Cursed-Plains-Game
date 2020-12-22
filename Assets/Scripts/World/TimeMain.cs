using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMain : MonoBehaviour
{

	WorldInfo wi;
	// Use this for initialization
	void Start ()
	{
		try {
			wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo> ();		

			GetComponent<Text> ().text = "Best Time: " + wi.bestTime + " Seconds";
		} catch {
			GetComponent<Text> ().text = "Best Time: " + 0 + " Seconds";
		}
	}
}
