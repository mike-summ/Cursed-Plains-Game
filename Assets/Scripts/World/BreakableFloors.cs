using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloors : MonoBehaviour {

	SpriteRenderer sr;
	public Sprite breaking;
	public float breakTimeDelay = 0.5f;

	float timer;
	bool startBreaking = false;
	GameObject player;
	PlayerController pc;
	string playerTag = "Player";

	void Start () {
		player = GameObject.FindGameObjectWithTag (playerTag);
		pc = player.GetComponent<PlayerController> ();
		sr = this.GetComponent<SpriteRenderer> ();

		timer = breakTimeDelay;
	}
	
	void Update () {
		if (startBreaking) {
			sr.sprite = breaking;

			if (timer <= 0) {
				Destroy (gameObject);
			} else {
				timer -= Time.deltaTime;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Equals (playerTag)) {
			if (pc.isSlamming) {
				startBreaking = true;
			}
		}
	}
}
