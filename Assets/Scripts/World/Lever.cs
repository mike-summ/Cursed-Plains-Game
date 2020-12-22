using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour 
{

	SpriteRenderer sr;

	public bool singleUse = false;
	public Sprite[] sprites;
	public GameObject target;
	public string methodName;

	private bool player = false;
	private bool used = false;

	void Start () 
	{
		sr = this.GetComponent<SpriteRenderer> ();	
		sr.sprite = sprites [0];
	}

	void Update ()
	{

		if (Input.GetButtonDown ("Interact") && player && !used) 
		{
			ExecuteLever ();
		}
	}

	void OnTriggerEnter2D(Collider2D col) 
	{
		if (col.tag == "Player" && !used) 
		{
			player = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) 
	{
		if (col.tag == "Player" && !used) 
		{
			player = false;
		}
	}

	void ExecuteLever () 
	{
		
		if (singleUse) 
		{
			used = true;	
		}

		if (sr.sprite == sprites [0]) 
		{
			sr.sprite = sprites [1];
		} else 
		{
			sr.sprite = sprites [0];
		}

		target.SendMessage (methodName);
	}
}
