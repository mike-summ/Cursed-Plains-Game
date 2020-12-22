using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {


	GameObject player;
	GameObject enemy;
	PlayerController controller;

	public bool primalUnlocked = false;
	public int primalCost = 20;

	// ATTACK
	public int direction; // 0 = right, 1 - left

	public GameObject[] fireball;
	private Transform shootPosition;

	private bool canAttack = true;

	public float delayTime = 0.5f;
	float timer = 0;

	void Start() {
		player = GameObject.Find ("Player");
		controller = player.GetComponent<PlayerController> ();
	}

	void Update() {
		direction = controller.GetDirection();

		// Switch the direction to the correct values
		if (direction == 0) {
			direction = 1;
		} else {
			direction = 0;
		}
			
		shootPosition = controller.wallCheck [direction].transform;

		if (canAttack) {
			if (Input.GetButtonDown ("Attack")) {
				Shoot (0);
				canAttack = false;
				timer = delayTime;
			} else if (Input.GetButtonDown ("Primal Attack") && primalUnlocked && controller.rage >= primalCost) {
				Shoot (1);
				controller.ChangeRage (-primalCost);
				canAttack = false;
				timer = delayTime;
			}
		} else {
			if (timer <= 0) {
				canAttack = true;
			} else {
				timer -= Time.deltaTime;
			}
		}
	}

	public void Shoot(int fireballType) {
		// Shooting logic

		Vector2 newPos;
		if (direction == 0) {
			newPos = new Vector2 (shootPosition.position.x - 0.6f, shootPosition.position.y);
			Instantiate (fireball [fireballType], newPos, Quaternion.Euler(335f,0f,0f));
		} else {
			newPos = new Vector2 (shootPosition.position.x + 0.6f, shootPosition.position.y);
			Instantiate (fireball [fireballType], newPos, Quaternion.Euler(25f,0f,0f));
		}
	}

	public void SlamAttack(int damage) {
		
	}
}
