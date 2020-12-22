using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour {

	PlayerController pc;
	CameraController cc;
	Rigidbody2D rb;

	public float startTime = 0f;
	public float timer = 0f;
	public float holdTime = 1f;
	public int cost = 40;

	private bool held = false;
	private bool canHeal = false;

	private string heal = "Heal";

	private int check = 1;

	void Start() {
		pc = this.GetComponent<PlayerController> ();
		cc = Camera.main.GetComponent<CameraController> ();
		rb = this.GetComponent<Rigidbody2D> ();
	}

	void Update () {
		if (pc.rage >= cost && pc.health < pc.healthMax) {
			canHeal = true;
		} else {
			canHeal = false;
		}
			
		if (canHeal) {
			// Starts the timer from when the key is pressed.
			if (Input.GetButtonDown (heal)) {
			
				startTime = Time.time;
				timer = startTime;
				pc.canMove = false;
				cc.ChangeSize (cc.GetSize() - 4f, holdTime);

				rb.velocity = new Vector2 (0, 0);
			}
				
			if (Input.GetButton (heal) && !held) {
				timer += Time.deltaTime;

				if (timer > (startTime + holdTime)) {
					held = true;
					HealPlayer ();
				}
			}

			if (Input.GetButtonUp (heal)) {
				held = false;
				cc.ResetSize ();
				pc.canMove = true;
				check = 1;
			}
		}
	}

	public void HealPlayer () {
		if (check > 0) {
			pc.Heal (1);
			pc.canMove = true;
			pc.rage -= cost;
			cc.ResetSize ();
			held = false;
			check--;
		}
	}
}
