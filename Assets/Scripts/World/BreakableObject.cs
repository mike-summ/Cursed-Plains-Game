using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {

	public GameObject remains;

	bool hit = false;

	public float destroyDelay = 1f;
	private float timer = 0;

	// Use this for initialization
	void Start () {
		timer = destroyDelay;
	}
	
	// Update is called once per frame
	void Update () {
		if (hit) 
		{
			if (timer <= 0) 
			{
				Destroy (this.gameObject);
			} else {
				timer -= Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.tag.Equals ("Fireball") && !hit) 
		{
			Break ();

			hit = true;
		} else if (col.tag.Equals ("Player") && !hit) 
		{
			if (col.GetComponent<PlayerController> ().Dashing ()) 
			{
				Break ();

				hit = true;
			}
		}
	}

	void Break() 
	{
		GameObject instRemains = Instantiate (remains, transform.position, Quaternion.identity);

		instRemains.transform.parent = this.transform;
		instRemains.GetComponent<Rigidbody2D> ().velocity = Vector2.up;

		this.GetComponent<SpriteRenderer> ().enabled = false;
	}
}
