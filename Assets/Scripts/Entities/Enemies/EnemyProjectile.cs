using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

	Rigidbody2D rb;

	public float speed = 15f;
	public int damage = 1;
	public GameObject particle;
	public int direction = 0; // 0 = top-left, 1 = top-right, 2 = bottom-left, 3 = bottom - right

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();

		switch (direction) {
		case 0:
			rb.velocity = new Vector2 (-1, 1) * speed;
			break;
		case 1:
			rb.velocity = new Vector2 (1, 1) * speed;
			break;
		case 2:
			rb.velocity = new Vector2 (-1, -1) * speed;
			break;
		case 3:
			rb.velocity = new Vector2 (1, -1) * speed;
			break;
		case 4:
			rb.velocity = new Vector2 (-1, 0) * speed;
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D hit) {
		if (hit.tag.Equals ("Player")) {
			hit.SendMessage ("Hit", damage);
			Instantiate (particle, transform.position, Quaternion.identity);

			Destroy (gameObject);
		} else if (hit.tag.Equals ("Ground")) {
			Instantiate (particle, transform.position, Quaternion.identity);

			Destroy (gameObject);
		} else if (hit.tag.Equals("Breakable")) {
			Instantiate (particle, transform.position, Quaternion.identity);

			Destroy (gameObject);
		}
	}

	void SetDirection(int no) {
		direction = no;
	}
}
