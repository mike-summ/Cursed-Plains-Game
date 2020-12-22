using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {

	Vector3 originalPos;
	PlayerController pc;
	private bool isTouching = false;
	public bool canHit = false;
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		originalPos = transform.position;
	}

	void Update () {
		if (isTouching && canHit) {
			pc.Hit (1);
		}
	}

	public void Reset () {
		transform.position = originalPos;
		canHit = false; 
		GetComponent<Animator> ().ResetTrigger ("attack");
		GetComponent<Animator> ().SetBool ("hasFinished", true);
	}

	public void AnimStart () {
		GetComponent<Animator> ().SetBool ("hasFinished", false);
	}

	public void CanHit() {
		canHit = true;
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag.Equals ("Player")) {
			isTouching = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag.Equals ("Player")) {
			isTouching = false;
		}
	}
}
