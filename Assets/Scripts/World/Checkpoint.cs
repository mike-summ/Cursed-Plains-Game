using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	PlayerController pc;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag.Equals("Player")) {
			pc = col.GetComponent<PlayerController> ();
			pc.checkpoint = transform.position;
		}
	}
}
