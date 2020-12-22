using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour {

	Rigidbody2D rb;
	SpriteRenderer sr;

	public int health = 1;
	public float speed = 5f;
	public enum EnemyType {
		Offensive, 
		Neutral
	};
	public EnemyType type;

	bool x_direction = false; // false = right, true = left
	bool y_direction = true; // false = down, true = up

	public GameObject projectile;
	public float attackDelay = 1f;
	float timer = 0;

	public LayerMask solidMask;
	public float checkRadius = 0.5f;

	public Transform[] detectors; // 0 = left, 1 = up, 2 = right, 3 = down

	public GameObject blood;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		sr = this.GetComponent<SpriteRenderer> ();
		timer = attackDelay;
	}
	
	// Update is called once per frame
	void Update () {
		if (type == EnemyType.Offensive) {
			if (timer <= 0) {
				Attack ();
				timer = attackDelay;
			} else {
				timer -= Time.deltaTime;
			}
		}

		sr.flipX = x_direction;

		if (health <= 0) {
			Death ();
		}
	}

	void FixedUpdate() {
		bool leftCheck = Physics2D.Raycast (detectors [0].position, Vector2.left, checkRadius, solidMask);
		bool upCheck = Physics2D.Raycast (detectors [1].position, Vector2.up, checkRadius, solidMask);
		bool rightCheck = Physics2D.Raycast (detectors [2].position, Vector2.right, checkRadius, solidMask);
		bool downCheck = Physics2D.Raycast (detectors [3].position, Vector2.down, checkRadius, solidMask);

		if (rightCheck) {
			x_direction = true;
		}
		if (leftCheck) {
			x_direction = false;
		}
		if (upCheck) {
			y_direction = false;
		}
		if (downCheck) {
			y_direction = true;
		}

		Move ();
	}

	void Attack() {
		GameObject proj0 = Instantiate (projectile, transform.position, Quaternion.identity);
		proj0.SendMessage ("SetDirection", 0, SendMessageOptions.DontRequireReceiver);

		GameObject proj1 = Instantiate (projectile, transform.position, Quaternion.identity);
		proj1.SendMessage ("SetDirection", 1, SendMessageOptions.DontRequireReceiver);

		GameObject proj2 = Instantiate (projectile, transform.position, Quaternion.identity);
		proj2.SendMessage ("SetDirection", 2, SendMessageOptions.DontRequireReceiver);

		GameObject proj3 = Instantiate (projectile, transform.position, Quaternion.identity);
		proj3.SendMessage ("SetDirection", 3, SendMessageOptions.DontRequireReceiver);
	}

	void Move() {
		float x_velocity;
		float y_velocity;

		if (x_direction) {
			x_velocity = -speed;
		} else {
			x_velocity = speed;
		}

		if (y_direction) {
			y_velocity = speed;
		} else {
			y_velocity = -speed;
		}

		rb.velocity = new Vector2 (x_velocity, y_velocity);
	}

	void Hit(int damage) {
		health -= damage;
	}

	void Death() {
		Instantiate (blood, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
