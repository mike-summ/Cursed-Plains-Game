using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceBoss : MonoBehaviour
{

	// 3 roots come up from the ground in succession. Fireballs can be shot, 1 above and 1 below. One root will follow the player
	// Stage 1 - Follow Root, Fireballs
	// Stage 2 - Three roots, Fireballs

	Animator princeAnim;
	Animator firstRoot;
	Animator secondRoot;
	Animator thirdRoot;
	Animator followRoot;
	PlayerController pc;
	GameObject player;

	public int health = 30;
	public int stage = 1;
	public GameObject[] roots;
	public GameObject fireball;
	public Transform topPosition;
	// To attack jumps
	public Transform bottomPosition;
	// To attack standing still

	public GameObject secondStage;
	public Transform leftSecondStage;
	public Transform rightSecondStage;

	public float attackDelay = 2f;
	private float attackTimer;

	public float rootDelay = 0.8f;
	private float rootTimer;

	private bool isTouching = false;
	private bool startBoss = false;
	private bool rootAttacking = false;
	private int rootAttack = 0;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		pc = player.GetComponent<PlayerController> ();

		princeAnim = GetComponent<Animator> ();
		firstRoot = roots [0].GetComponent<Animator> ();
		secondRoot = roots [1].GetComponent<Animator> ();
		thirdRoot = roots [2].GetComponent<Animator> ();
		followRoot = roots [3].GetComponent<Animator> ();
		attackTimer = attackDelay;
		rootTimer = rootDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (startBoss) {
			if (isTouching) {
				pc.Hit (1);
			}

			if (health <= 25) {
				stage = 2;
			}

			if (health <= 0) {
				Death ();
			}

			if (attackTimer <= 0) {
				if (stage == 1) {
					FollowRootAttack ();
					FireballAttack ();
				} else {
					float value = Random.value;
					if (value <= 0.6f) {
						ThreeRootAttack ();
						FireballAttack ();
					} else {
						FireballAttack ();
					}
				}
				attackTimer = attackDelay;
			} else {
				attackTimer -= Time.deltaTime;
			}

			if (rootAttacking) {
				if (rootTimer <= 0) {
					if (rootAttack >= 3) {
						rootAttacking = false;
					} else {
						switch (rootAttack) {
						case 1:
							secondRoot.SetTrigger ("attack");
							break;
						case 2:
							thirdRoot.SetTrigger ("attack");
							break;
						}
						rootAttack++;
					}
					rootTimer = rootDelay;
				} else {
					rootTimer -= Time.deltaTime;
				}
			}
		}
	}

	void FollowRootAttack ()
	{
		// follow root
		try {
			roots [3].transform.parent.position = new Vector3 (player.transform.position.x, roots [3].transform.parent.position.y, roots [3].transform.parent.position.z);
			followRoot.SetTrigger ("attack"); // Script for root that resets the trigger
		} catch {
		}
	}

	void FireballAttack ()
	{
		// fireball attack
		GameObject proj;
		GameObject proj1;

		proj = Instantiate (fireball, topPosition.transform.position, Quaternion.identity);
		proj1 = Instantiate (fireball, bottomPosition.transform.position, Quaternion.identity);

		proj.SendMessage ("SetDirection", 4, SendMessageOptions.DontRequireReceiver);
		proj1.SendMessage ("SetDirection", 4, SendMessageOptions.DontRequireReceiver);
	}

	public bool hasFininished = false;

	void ThreeRootAttack ()
	{
		// three roots attack
		rootAttacking = true;
		rootAttack = 1;

		if (thirdRoot.GetBool ("hasFinished")) {
			float value = Random.value;
			if (value <= 0.5f) {
				secondStage.transform.position = leftSecondStage.position;
			} else {
				secondStage.transform.position = rightSecondStage.position;
			}
		}

		firstRoot.SetTrigger ("attack");
	}

	public void Hit (int damage)
	{
		health -= damage;
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag.Equals ("Player")) {
			isTouching = true;
		}
	}

	void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.tag.Equals ("Player")) {
			isTouching = false;
		}
	}

	void Death ()
	{
		// Death shenanigans

		CameraController cc = Camera.main.GetComponent<CameraController> ();
		cc.BossDead ();
		Destroy (transform.parent.gameObject);
		Animator an = GameObject.FindGameObjectWithTag ("DeathFtoB").GetComponent<Animator> ();
		an.SetBool ("win", true);

		WorldInfo wi = GameObject.FindGameObjectWithTag ("WorldInfo").GetComponent<WorldInfo>();
		wi.Finish ();
	}

	public void EnterBossRoom ()
	{
		startBoss = true;
	}
}
