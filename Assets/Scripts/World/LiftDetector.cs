using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDetector : MonoBehaviour {

	GameObject player;
	public GameObject lift;

	void Start() 
	{
		player = GameObject.FindGameObjectWithTag ("Player");	
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.tag.Equals("Player")) 
		{
			this.SendMessageUpwards ("ActivateLift");
			player.transform.parent = lift.transform;
		}
	}

	void OnTriggerExit2D (Collider2D col) 
	{
		player.transform.parent = null;
	}

}