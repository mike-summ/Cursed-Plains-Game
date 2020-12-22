using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeath : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag.Equals ("Player")) {
			col.GetComponent<PlayerController> ().health = 0;
		}
	}
}
