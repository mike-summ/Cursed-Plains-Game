using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	Camera playerCamera;
	CameraController cShake;
	public float speed = 20f;
	public int damage = 1;
	public Rigidbody2D rb;
	public GameObject fireParticle;
	public bool isPrimal = false;
	public int lifeAmount = 1;

	PlayerController controller;

	// Use this for initialization
	void Start () {
		
		controller = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		if (!isPrimal) {
			if (controller.GetDirection () == 0) {
				rb.velocity = new Vector2 (1, 0.1f) * speed;
			} else {
				rb.velocity = new Vector2 (-1, 0.1f) * speed;
			}
		} else {
			if (controller.GetDirection () == 0) {
				rb.velocity = new Vector2 (1, 0) * speed;
			} else {
				rb.velocity = new Vector2 (-1, 0) * speed;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D hitInfo) {
		if (hitInfo.tag == "Enemy") {
			playerCamera = Camera.main;
			cShake = playerCamera.GetComponent<CameraController> ();
			cShake.TriggerShake (0.1f, 2f, 0.2f);
			Instantiate (fireParticle, transform.position, Quaternion.identity);

			if (!isPrimal) {
				controller.ChangeRage (10);
			}

			hitInfo.SendMessage ("Hit", damage);

			if (lifeAmount <= 0) {
				Destroy (gameObject);
			} else {
				lifeAmount--;
			}
		} else if (hitInfo.tag == "Ground") {
			playerCamera = Camera.main;
			cShake = playerCamera.GetComponent<CameraController> ();
			cShake.TriggerShake (0.1f, 2f, 0.2f);
			Instantiate (fireParticle, transform.position, Quaternion.identity);

			Destroy (gameObject);
		} else if (hitInfo.tag == "Breakable") {
			playerCamera = Camera.main;
			cShake = playerCamera.GetComponent<CameraController> ();
			cShake.TriggerShake (0.1f, 2f, 0.2f);
		}
	}
}
