using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntrance : MonoBehaviour {
	CaveFade cf;
	GameObject caveFade;
	// Use this for initialization
	void Start () {
		caveFade = GameObject.FindGameObjectWithTag ("Cave");
		cf = caveFade.GetComponent<CaveFade> ();
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag.Equals ("Player")) {
			cf.lightSwitch ();
		}
	}
}
