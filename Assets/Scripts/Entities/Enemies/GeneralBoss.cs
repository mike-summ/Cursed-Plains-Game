using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBoss : MonoBehaviour
{

	/*
	 *	This boss will test the player on their Movement skills (Jump and WASD).
	 *	How to do this?
	 *	- Platforms on either side for the character to use when the boss charges. (To jump over).
	 *	- Boss weakpoint is his head, requires the player to jump and shoot.
	 *	- Second half of the boss fights has a new move, the smash attack. The general kicks down on the floor and fire shoots out towards the player. They will have to jump over it.
	*/

	GameObject player;
	Rigidbody2D rb;
	SpriteRenderer sr;
	public BossRoom br;

	private int maxHealth;
	public int health = 20;
	public float speed = 5f;
	private float normalSpeed;
	public int chargeDamage = 1;
	public int slamDamage = 1;

	public float delay = 0.5f;
	private float timer;

	public bool isCharging = false;
	public int direction = 1;
	// 1 = right, -1 = left

	public float chargeSpeed = 10f;
	public float attackTimer = 0f;
	public float attackTimerDelay = 1f;

	public float chargeTimer = 0f;
	float chargeDelay = 3f;

	public bool isTouching = false;
	public bool startBoss = false;

	private int tempDir;

	public GameObject bossWalls;
	Transform startPosition;

	public float hitDistance = 5f;

	// If boss is too close to the sides and he is facing them, he'll jump back.

	GameObject[] sides;
	// Empty gameobjects on either side of the arena.
	public float closeDistance = 5f;

	// Use this for initialization
	void Start ()
	{
		maxHealth = health;
		startPosition = transform;
		player = GameObject.FindGameObjectWithTag ("Player");
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		timer = delay;
		attackTimer = attackTimerDelay;
		chargeTimer = chargeDelay;
		normalSpeed = speed;
	}

	void Update ()
	{
		if (DistanceFromTarget (player) < hitDistance) {
			isTouching = true;
		} else {
			isTouching = false;
		}

		if (isTouching) {
			try {
			player.gameObject.GetComponent<PlayerController> ().Hit (1);
			} catch {
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{

		if (health <= 0) {
			Death ();
		}

		// Moves towards player
		// If is certain distance away AND attackTimer is less than 0, charge

      
		if (startBoss) {

			if (attackTimer <= 0) {
               
				isCharging = true;  
			} else {
				attackTimer -= Time.deltaTime;
			}


			// Direction
			if (!isCharging) {
				float pX = player.transform.position.x;
				float bX = transform.position.x;
				if (pX < bX) {
					direction = -1;
					sr.flipX = true;
				} else {
					direction = 1;
					sr.flipX = false;
				}
			} else {
				speed = chargeSpeed;
				if (chargeTimer <= 0) {
					speed = normalSpeed;
					isCharging = false;
					attackTimer = attackTimerDelay;
					chargeTimer = chargeDelay;
				} else {
					chargeTimer -= Time.deltaTime;
				}
			}
			rb.velocity = new Vector2 (speed * direction, rb.velocity.y);
            
		}
	}


	public void Hit (int dmg)
	{
		health -= dmg;
	}

	float DistanceFromTarget (GameObject target)
	{
		try {
			float distance;
			float pX = target.transform.position.x;
			float pY = target.transform.position.y;

			float bX = transform.position.x;
			float bY = transform.position.y;

			distance = Mathf.Sqrt (Mathf.Pow (bX - pX, 2) + Mathf.Pow (bY - pY, 2));

			return distance;
		} catch {
			return 0;
		}
	}


	void Death ()
	{
		// Death shenanigans
	
		WorldInfo wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo> ();
		wi.bossNames.Add (gameObject.name);

		CameraController cc = Camera.main.GetComponent<CameraController> ();
		cc.BossDead ();
        
		player.GetComponent<PlayerController> ().amountOfJumps = 2;
		Destroy (gameObject);
	}

	void AlreadyDead ()
	{
		// Death shenanigans

		WorldInfo wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo> ();
		wi.bossNames.Add (gameObject.tag);

		bossWalls.SetActive (false);
		Destroy (gameObject);
	}

	public void EnterBossRoom ()
	{
		startBoss = true;
	}

	public void Reset ()
	{
		health = maxHealth;
		isTouching = false;
		transform.parent.position = startPosition.position;
		startBoss = false;
	}
}
