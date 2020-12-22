using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

	// Player Information

	[Header ("Player Information")]

	public int healthMax = 5;
	public int health = 5;
	public int rage = 100;
	public int rageMax = 100;
	public int rageOverTime = 5;
	private float rageTime;

	// Things to do tomorrow: Attacking and Slam Damage. Also I-Frames

	private Rigidbody2D rb;

	// BASIC INPUTS

	[Header ("Basic Parameters")]

	public GameObject playerCamera;
	CameraController cShake;
	public float speed;
	public float jumpForce;
	public float airJumpForce;
	private float moveInput;

	// DASH

	[Header ("Dash")]

	private bool isDashing = false;
	public int dashCost = 20;
	public float dashSpeed;
	public float dashDelayTime;
	private float dashTime;
	public GameObject smokeTrail;

	public GameObject[] dashTrail;
	// 1 = left, 0 = right

	private bool canDash = true;

	// BOOLS

	public bool canMove = true;

	// GROUND CHECK

	[Header ("Ground Check")]

	bool isGrounded;
	bool earlyJump;
	public Transform groundCheck;
	public float checkGroundRadius;
	public LayerMask solidMask;

	// WALL CHECK

	[Header ("Wall Check")]

	bool isOnWall;
	bool isOnLeftWall;
	bool isOnRightWall;
	public Transform[] wallCheck;
	public float checkWallRadius;
	public float wallSlideVelocity;
	public float wallSlideDuration;
	private bool isWallSliding;
	private float slideDuration;

	// JUMPS

	[Header ("Jump")]

	public int amountOfJumps;
	public int amountOfWallJumps;
	public int wallJumps;
	public int extraJumps;
	private bool canJump;
	private bool futureJump;

	// SLAM

	[Header ("Slam")]

	public bool canSlam;
	public float slamForce;
	public float slamRadius;
	public bool isSlamming;
	private bool isTouching;

	// MOVEMENT CHECKS

	private Vector2 lastPos;
	private Vector2 currPos;

	// ANIMATOR

	Animator anim;

	// I-Frames

	private bool vulnerable = false;
	float vTimer = 0f;
	public float vDelayTime = 1f;

	// LEVEL
	LevelController lc;
	public GameObject levelController;

	[Header ("Respawn")]

	public Vector2 checkpoint;
	public GameObject blood;

	void Start ()
	{
		WorldInfo wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo> ();
		Attack a = gameObject.GetComponent<Attack> ();

		a.primalUnlocked = wi.primal;
		amountOfJumps = wi.noOfJumps;
		checkpoint = wi.checkpoint;
		transform.position = checkpoint;


		lc = levelController.GetComponent<LevelController> ();
		rb = GetComponent<Rigidbody2D> ();
		extraJumps = amountOfJumps - 1;
		cShake = playerCamera.GetComponent<CameraController> ();
		anim = this.GetComponent<Animator> ();
		futureJump = false;
		SetVulnerable (true);
		vTimer = vDelayTime;
	}

	void FixedUpdate ()
	{
		// Checks for the player's collision.
		// groundedCheck
		bool check1 = Physics2D.Raycast (new Vector2 (groundCheck.position.x - 0.8f, groundCheck.position.y), Vector2.down, checkGroundRadius, solidMask);
		bool check2 = Physics2D.Raycast (groundCheck.position, Vector2.down, checkGroundRadius, solidMask);
		bool check3 = Physics2D.Raycast (new Vector2 (groundCheck.position.x + 0.8f, groundCheck.position.y), Vector2.down, checkGroundRadius, solidMask);

		if (check1 || check2 || check3) {
			isGrounded = true;
		} else {
			isGrounded = false;
		}

		isOnLeftWall = Physics2D.Raycast (wallCheck [0].position, Vector2.left, checkWallRadius, solidMask);
		isOnRightWall = Physics2D.Raycast (wallCheck [1].position, Vector2.right, checkWallRadius, solidMask);

		bool earlyCheck = Physics2D.Raycast (groundCheck.position, Vector2.down, checkGroundRadius + 2f, solidMask);


		if (earlyCheck && rb.velocity.y < 0) {
			earlyJump = true;
		} else {
			earlyJump = false;
		}

		if (canMove) {
			moveInput = Input.GetAxis ("Horizontal");
			rb.velocity = new Vector2 (moveInput * speed, rb.velocity.y);
		}

		// Determines if the player is moving right or not, and flips the sprite accordingly.
		if (!isOnWall) {
			if (moveInput > 0) {
				Flip (0);
				if (isGrounded) {
					anim.SetBool ("isRunning", true);
				} else {
					anim.SetBool ("isRunning", false);
				}
			} else if (moveInput < 0) {
				Flip (1);
				if (isGrounded) {
					anim.SetBool ("isRunning", true);
				} else {
					anim.SetBool ("isRunning", false);
				}
			} else if (moveInput == 0) {
				anim.SetBool ("isRunning", false);
			}
		}


		// DASH

		if (canDash) {
			if (Input.GetButton ("Dash") && rage >= dashCost) {
				isDashing = true;
				Instantiate (smokeTrail, transform.position, Quaternion.identity);

				cShake.TriggerShake (0.05f, 2f, 0.1f);

				dashTime = dashDelayTime;
				canMove = false;
				rb.velocity = Vector2.zero;

				if (GetDirection () == 0) {
					rb.velocity = new Vector2 (dashSpeed, 0);
					dashTrail [1].SetActive (true);
				} else {
					rb.velocity = new Vector2 (-dashSpeed, 0);
					dashTrail [0].SetActive (true);
				}

				canMove = true;
				canDash = false;

				rage -= dashCost;
			}
		} else {
			if (dashTime <= 0) {
				if (isGrounded) {
					canDash = true;
				}
			} else {
				dashTime -= Time.deltaTime;
				rb.velocity = Vector2.zero;
			}
		}

		if (dashTime <= 0) {
			dashTrail [0].SetActive (false);
			dashTrail [1].SetActive (false);
			isDashing = false;
		} else {
			dashTime -= Time.deltaTime;
		}

		if (GetVulnerability () == false) {
			if (vTimer <= 0) {
				SetVulnerable (true);
				vTimer = vDelayTime;
				this.GetComponent<SpriteRenderer> ().color = Color.white;
			} else {
				vTimer -= Time.deltaTime;
				this.GetComponent<SpriteRenderer> ().color = Color.red;
			}
		}

	}

	void Update ()
	{

		// Checks if the player is on the wall and is not grounded.
		if ((isOnLeftWall || isOnRightWall) && !isGrounded) {
			isOnWall = true;
		} else {
			isOnWall = false;
		}

		// Resets the amount of jumps and wall Jumps when the player is on the ground.
		if (isGrounded) {
//			anim.SetBool ("isGrounded", true);
			extraJumps = amountOfJumps - 1;
			wallJumps = amountOfWallJumps;
		} else {
			anim.SetBool ("isGrounded", false);
			anim.SetBool ("isJumping", false);
		}

		// Checks if the player can jump or not.
		if (extraJumps > 0) {
			canJump = true;
		} else {
			canJump = false;
		}

		// Checks if the jump is on a wall or not.
		if (!isSlamming) {
			if (!isOnWall) {
				//ChangeMovement (true);
				if (Input.GetButtonDown ("Jump") && !isGrounded) { // The player jumps in the air.
					if (!earlyJump) {
						if (canJump) {
							Jump (airJumpForce);
							extraJumps--;
							cShake.TriggerShake (0.05f, 2f, 0.1f);
							Instantiate (smokeTrail, transform.position, Quaternion.identity);
						}
					} else {
						futureJump = true;
					}
				} else if (Input.GetButtonDown ("Jump") && isGrounded) { // The player jumps on the ground.
					Jump (jumpForce);
					anim.SetBool ("isJumping", true);
				}
			} else {
				if (Input.GetButtonDown ("Jump")) { // The player jumps off the wall.
					if (wallJumps > 0) {
						Jump (jumpForce);
						wallJumps--;
					} else if (canJump) {
						Jump (airJumpForce);
						extraJumps--;
						cShake.TriggerShake (0.05f, 2f, 0.1f);
					}
				}
			}
		}

		// Slam attack
		if (Input.GetButtonDown ("Slam") && !isGrounded && !isSlamming) {
			Slam ();
		}

		if (isSlamming && (isGrounded || isTouching)) {
			cShake.TriggerShake (0.15f, 2f, 0.2f);
			isSlamming = false;
			ChangeMovement (true);
		}

		// Wall Slide

		if (isOnWall && !isGrounded && !isSlamming) {
			isWallSliding = true;
		} else {
			isWallSliding = false;
		}

		if (isWallSliding) {
			if (slideDuration > 0) {
				slideDuration -= Time.deltaTime;
				if (rb.velocity.y < -wallSlideVelocity) {
					rb.velocity = new Vector2 (rb.velocity.x, -wallSlideVelocity);
				}
			} else {
				rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y);
			}
		} else {
			slideDuration = wallSlideDuration;
		}

		// Jumps if the player has buffered the jump button before they hit the ground
		if (futureJump && isGrounded) {
			Jump (jumpForce);
			futureJump = false;
		}

		// Rage filling up
		if (rageTime <= 0) {
			rage += rageOverTime;
			rageTime = 1f;
		} else {
			rageTime -= Time.deltaTime;
		}

		if (rage >= rageMax) {
			rage = rageMax;
		}

		if (health <= 0) {
			Death ();
		}

		if (health > healthMax) {
			health = healthMax;
		}

	}

	void Flip (int direction)
	{ // Flips the player's sprite.
		SpriteRenderer spriteR = GetComponent<SpriteRenderer> ();

		if (direction == 0) {
			spriteR.flipX = false;
		} else {
			spriteR.flipX = true;
		}
	}

	void Slam ()
	{
		if (canSlam) {
			ChangeMovement (false);
			isSlamming = true;
			rb.velocity = Vector2.down * slamForce;
		}
	}

	public void Jump (float force)
	{
		rb.velocity = Vector2.up * force;
	}

	public void ChangeMovement (bool movementValue)
	{
		canMove = movementValue;
	}

	public int GetDirection ()
	{
		SpriteRenderer spriteR = GetComponent<SpriteRenderer> ();
		if (spriteR.flipX) {
			return 1;
		} else {
			return 0;
		}
	}

	public void SetVulnerable (bool vulnerabilityValue)
	{
		vulnerable = vulnerabilityValue;
	}

	public bool GetVulnerability ()
	{
		return vulnerable;
	}

	public void ChangeRage (int change)
	{
		rage += change;
	}

	public bool Dashing ()
	{
		return isDashing;
	}

	public void Hit (int damage)
	{
		if (GetVulnerability ()) {
			try {
				health -= damage;

				rb.velocity = new Vector2 (0, 0);
				Jump (10);
				cShake.TriggerShake (0.1f, 2f, 0.2f);

				SetVulnerable (false);
			} catch {
			}
		}
	}

	public void Death ()
	{
		// Dead shenanigans here
		Instantiate (blood, transform.position, Quaternion.identity);
		Animator an = GameObject.FindGameObjectWithTag ("DeathFtoB").GetComponent<Animator> ();
		an.SetBool ("death", true);
		Destroy (gameObject);
	}

	public void Heal (int amount)
	{
		health += amount;
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag.Equals ("Enemy")) {
			isTouching = true;
		} else if (col.gameObject.tag.Equals ("Ground")) {
			lc.UpdateLevel (col.gameObject.name);
		}
	}

	void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.tag.Equals ("Enemy")) {
			isTouching = false;
		}
	}
}
