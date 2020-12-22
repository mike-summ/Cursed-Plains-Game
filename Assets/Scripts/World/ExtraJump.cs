using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : MonoBehaviour {

	SpriteRenderer sr;

	public float delay = 3f;
	float timer = 0f;
	bool hit = false;

	void Start() {
		sr = this.GetComponent<SpriteRenderer> ();
		timer = delay;
	}

	void Update() {
		if (hit) {
			if (timer <= 0) {
				hit = false;
				timer = delay;
				sr.enabled = true;
			} else {
				timer -= Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag.Equals ("Player") && !hit) {
			PlayerController pc = col.GetComponent<PlayerController> ();
			pc.extraJumps++;
			hit = true;
			sr.enabled = false;
		}
	}
}
