using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveFade : MonoBehaviour {

	Animator anim;
	private bool inCave = true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.SetBool ("inCave", inCave);
	}

	public void lightSwitch() {
		inCave = !inCave;
		anim.SetBool ("inCave", inCave);
	}
}
