using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

	public bool liftBroken = false;
	
	GameObject player;
	public Transform lift;
	public Transform topLocation;
	public GameObject invisibleWall;
	Vector2 bottomLocation;

	public bool top;
	public float liftSpeed;
	public float liftDelayTime = 0.5f;
	private float liftTime;
	private bool liftActivated;

	void Start () 
	{
		if (!liftBroken) {
			bottomLocation = new Vector2 (lift.position.x, lift.position.y);
			top = false;
		} else {
			bottomLocation = topLocation.position;
			topLocation.position = new Vector2 (lift.position.x, lift.position.y);
			top = true;
		}

		invisibleWall.SetActive (false);
		liftTime = liftDelayTime;
	}

	void Update () 
	{
		if (!liftBroken) {
			if (liftActivated) {
				if (liftTime <= 0) {
					MoveLift ();	
				} else {
					liftTime -= Time.deltaTime;
				}
			}
		}
	}

	// Change positions of Lift
	public void MoveLift() 
	{
		if (top) 
		{
			if (lift.position.y >= bottomLocation.y) 
			{
				lift.Translate (Vector2.down * liftSpeed * Time.deltaTime);
				invisibleWall.SetActive (true);

			} else 
			{
				lift.position = bottomLocation;
				liftActivated = false;
				liftTime = liftDelayTime;
				top = false;
				invisibleWall.SetActive (false);
			}
		} else
		{
			if (lift.position.y <= topLocation.position.y) 
			{
				lift.Translate(Vector2.up * liftSpeed * Time.deltaTime);
			} else 
			{
				liftActivated = false;
				liftTime = liftDelayTime;
				top = true;
			}
		}
	}

	public void LeverLift () 
	{
		if (top) 
		{
			liftActivated = true;
			liftBroken = false;
		}
	}

	public void ActivateLift ()
	{
		liftActivated = true;
	}

}
