using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBoss : MonoBehaviour {

	// Boss ideas:
	// Movement focused, boss in the centre of the arena.
	// Bullet hell, enemies spawn.
	// Platforms around the boss.

	GameObject player;
	PlayerController pc;

	public int health = 30;
	public float attackDelay = 3f;
	private float attackTimer = 3f;
	public int stage = 1;

	public GameObject projectile;
	public GameObject zombieSpawn;
	public Transform[] spawnLocations;
	public GameObject[] lasers;

	private bool isTouching = false;
	private bool startBoss = false;

	private bool isAttacking = false;

	private bool isSpiral = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pc = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (startBoss) {

			if (stage == 1) {
				
			} else {
			
			}

			if (isTouching) {
				pc.Hit (1);
			}
		}

		if (isSpiral) {
			
		}
	}

	// Attack
	void SpawnEnemies() {
		for (int i = 0; i < spawnLocations.Length; i++) {
			Instantiate (zombieSpawn, spawnLocations [i].transform.position, Quaternion.identity);
		}
	}

	// Firing fireballs in a star formation
	void StarAttack() {
		// 8 shorts in all directions
		Vector2[] direction = {
			new Vector2(-1,0), 
			new Vector2(-1,1), 
			new Vector2(0,1), 
			new Vector2(1,1), 
			new Vector2(1,0), 
			new Vector2(1,-1), 
			new Vector2(0,-1), 
			new Vector2(-1,-1)
		};

		for (int i = 0; i < 8; i++) {
			GameObject proj = Instantiate (projectile, transform.position, Quaternion.identity);
			proj.SendMessage ("Direction", direction[i]);
		}
		isAttacking = false;
	}

	// Firing fireballs in a spiral
	void SpiralAttack() {
		isSpiral = true;
	}

	// Laser Attack?
	void LaserAttack() {
		isAttacking = false;
	}

	// Touching
	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Player")) {
			isTouching = true;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Player")) {
			isTouching = false;
		}
	}

	void Hit(int damage) {
		health -= damage;
	}

	void Death() {
		// Death shenanigans

		CameraController cc = Camera.main.GetComponent<CameraController> ();
		cc.BossDead ();
		Destroy (transform.parent.gameObject);
	}

	public void EnterBossRoom() {
		startBoss = true;
	}
}
