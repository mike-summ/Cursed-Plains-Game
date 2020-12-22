using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	[Header ("Enemy Information")]

	PlayerController pc;
	SpriteRenderer sr;

	public int Health = 3;
	public float AttackTime = 1f;
	public int Damage = 1;

	public enum EnemyType
	{
		Shield,
		Bandit,
		Draugr}

	;

	public EnemyType eType;

	public int status;
	// 0 = idle, 1 = roaming, 2 = alert, 3 = stunned
	public float speed;
	public float idleSpeed = 5f;
	public float alertSpeed = 7f;
	public float attackSpeed = 8f;
	bool direction = true;

	public int stunAmount = 2;
	int stun;

	public Transform rightCheck;
	public Transform leftCheck;
	public float checkDistance;
	public float ledgeCheckDistance;
	public LayerMask solidMask;
	public LayerMask playerMask;

	private bool leftCheckAll;
	private bool rightCheckAll;

	public GameObject blood;

	public float stunHitDelay = 1.3f;
	private float stunTimer = 0f;

	public float hitDelay = 0.8f;
	private float hitTimer = 0f;

	bool hit = false;

	bool isAttacking = false;
	float attackTimer;
	public float attackTimerDelay = 0.8f;
	public float attackChance = 0.2f;

	private float pauseTimer;
	private float pauseTimerDelay = 0.8f;

	public float hitDistance = 3f;
	private bool isTouching = false;


	Rigidbody2D rb;

	void Start ()
	{
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = this.GetComponent<SpriteRenderer> ();
		stun = stunAmount;
		attackTimer = attackTimerDelay;
		pauseTimer = pauseTimerDelay;
	}

	void FixedUpdate ()
	{
		if (status == 3) {
			if (stunTimer <= 0) {
				status = 0;
				stunTimer = stunHitDelay;
			} else {
				stunTimer -= Time.deltaTime;
				speed = 0;
				rb.velocity = new Vector2 (0, rb.velocity.y);
			}
		}

		if (hit) {
			if (hitTimer <= 0) {
				hit = false;
				hitTimer = hitDelay;
			} else {
				hitTimer -= Time.deltaTime;
				speed = 0;
				rb.velocity = new Vector2 (0, rb.velocity.y);
			}
		}

		if (Health <= 0) {
			Death ();
		}

		// If the enemy is idle, it does not move. If it detects a player, it starts moving towards the player. If the player disappears then it starts roaming.
		bool lenemyCheck = Physics2D.Raycast (leftCheck.position, Vector2.left, 15f, playerMask);
		bool renemyCheck = Physics2D.Raycast (rightCheck.position, Vector2.right, 15f, playerMask);

		if (status != 3) {
			if (lenemyCheck || renemyCheck || isAttacking || DistanceFromPlayer () < 3f) {
				status = 2;
				Debug.Log ("Why?");
			} else if (status == 2 && !(lenemyCheck || renemyCheck)) {
				status = 1;
			}
		}

		// Enemy collision checks
		bool lwallCheck = Physics2D.Raycast (leftCheck.position, Vector2.left, checkDistance, solidMask);
		bool lgroundCheck = Physics2D.Raycast (leftCheck.position, Vector2.down, ledgeCheckDistance, solidMask);

		bool rwallCheck = Physics2D.Raycast (rightCheck.position, Vector2.right, checkDistance, solidMask);
		bool rgroundCheck = Physics2D.Raycast (rightCheck.position, Vector2.down, ledgeCheckDistance, solidMask);

		try {
			if (status == 0) {
				rb.velocity = new Vector2 (0, rb.velocity.y);
			} else if (status == 1) {
				speed = idleSpeed;
				// Roaming
				if (lwallCheck || !lgroundCheck) {
					leftCheckAll = true;
				} else {
					leftCheckAll = false;
				}
				if (rwallCheck || !rgroundCheck) {
					rightCheckAll = true;
				} else {
					rightCheckAll = false;
				}

				if (rightCheckAll) {
					ChangeDirection (false);
				} else if (leftCheckAll) {
					ChangeDirection (true);
				}
				Move (direction);
			} else if (status == 2) { // Alert of the player
				float playerX = GameObject.FindGameObjectWithTag ("Player").transform.position.x;
				float enemyX = this.transform.position.x;

				bool ableToMove = false;

				if (eType == EnemyType.Draugr) { // Draugr
					speed = alertSpeed;
					if (playerX < enemyX) {
						ChangeDirection (false);
					} else {
						ChangeDirection (true);
					}
					Move (direction);

				} else if (eType == EnemyType.Bandit) { // Bandit
					speed = alertSpeed;

					if (!isAttacking) {
						if (DistanceFromPlayer () > 10f) {
							if (playerX < enemyX) {
								ChangeDirection (false);
							} else {
								ChangeDirection (true);
							}
							Move (direction);
						} else {
							if (Random.value <= attackChance) {
								rb.velocity = new Vector2 (0, 0);
								isAttacking = true;
							}
						}
					} else {
						if (pauseTimer <= 0) {
							Attack ();
						} else {
							pauseTimer -= Time.deltaTime;
						}
					}
				} else if (eType == EnemyType.Shield) { // Shield
					speed = alertSpeed;

					if (DistanceFromPlayer () > 5f) {
						if (playerX < enemyX) {
							ChangeDirection (false);
						} else {
							ChangeDirection (true);
						}
						Move (direction);
					} else {
						rb.velocity = new Vector2 (0, 0);
					}
				}
			}
		} catch {
			Debug.Log ("Error");
		}
	}

	void Update ()
	{
		if (DistanceFromPlayer () < hitDistance) {
			isTouching = true;
		} else {
			isTouching = false;
		}

		if (isTouching) {
			status = 3;
			isAttacking = false;
			pc.Hit (Damage);
		}
	}

	void Move (bool movDirection) // true = right, false = left
	{ 
		if (!hit) {
			if (movDirection) {
				rb.velocity = new Vector2 (speed, rb.velocity.y);
			} else {
				rb.velocity = new Vector2 (-speed, rb.velocity.y);
			}
		}
	}

	void ChangeDirection (bool newDirection)
	{
		direction = newDirection;
		sr.flipX = !direction;
	}

	float DistanceFromPlayer ()
	{
		try {
			float playerX = GameObject.FindGameObjectWithTag ("Player").transform.position.x;
			float playerY = GameObject.FindGameObjectWithTag ("Player").transform.position.y;
			float enemyX = this.transform.position.x;
			float enemyY = this.transform.position.y;

			float x = playerX - enemyX;
			float y = playerY - enemyY;
			float distance = Mathf.Sqrt ((x * x) + (y * y));

			return distance;
		} catch {
			return 0;
		}
	}

	public void Hit (int damage)
	{
		Health -= damage;
		hit = true;
		rb.velocity = Vector2.up * 10;

		if (status == 0) {
			status = 1;
		} else {
			if (eType != EnemyType.Shield && stun <= 0) {
				status = 3;
				stun = stunAmount;
			} else {
				stun--;
			}
		}
		//sr.sprite = hit;
	}

	public void Attack ()
	{
		speed = attackSpeed;

		if (attackTimer <= 0) {
			rb.velocity = new Vector2 (0, 0);
			isAttacking = false;
			attackTimer = attackTimerDelay;
			pauseTimer = pauseTimerDelay;
			speed = alertSpeed;
		} else {
			attackTimer -= Time.deltaTime;
			Move (direction);
		}
	}

	/*
	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag.Equals ("Player")) {
			isTouching = true;
		}
	}

	void OnCollisionExit2D (Collision2D col) {

		Debug.Log ("Working?" + isTouching);

		if (col.gameObject.tag.Equals ("Player")) {
			isTouching = false;
		}
	}
*/
	public void Death ()
	{
		if (eType == EnemyType.Shield) {
			eType = EnemyType.Bandit;
			Health = 1;
		} else {
			Instantiate (blood, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
		
}
