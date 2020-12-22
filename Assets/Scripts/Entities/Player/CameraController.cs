using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Camera playerCam;
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform player;

	public float shakeDuration = 0f;
	public float shakeMagnitude = 0.2f;
	public float dampingSpeed = 1.0f;
	Vector3 initialPosition;
	private bool canShake = false;
	public float defaultCamSize = 10f;

	// Camera Size
	bool resetting;
	bool changing;
	float newSize;
	float sizeTime;

	float curVel = 0f;

	private Transform playerTarget;

	public float bossDeathDelay = 2f;
	private float bossTimer;
	private bool bossDeath = false;


	void OnEnable () 
	{
		
		playerCam = this.GetComponent<Camera> ();
		playerTarget = player;
		bossTimer = bossDeathDelay;

		// Sets the position of the camera
		Vector3 point = playerCam.WorldToViewportPoint (player.position);
		Vector3 delta = player.position - playerCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = destination;
	}

	void Update () 
	{
		
		if (canShake) 
		{
			if (shakeDuration > 0) 
			{
				transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

				shakeDuration -= Time.deltaTime * dampingSpeed;
			} else 
			{
				shakeDuration = 0f;
				transform.localPosition = initialPosition;
				canShake = false;
			}
		}

		if (resetting) 
		{
			playerCam.orthographicSize = Mathf.SmoothDamp (playerCam.orthographicSize, defaultCamSize, ref curVel, 0.2f);
		}
		if (changing) 
		{
			playerCam.orthographicSize = Mathf.SmoothDamp (playerCam.orthographicSize, newSize, ref curVel, sizeTime);
		}

		if (bossDeath) {
			if (bossTimer <= 0) {
				player = playerTarget;
				defaultCamSize = 18f;
				ResetSize ();
				bossTimer = bossDeathDelay;
				bossDeath = false;
			} else {
				bossTimer -= Time.deltaTime; 
			}
		}
	}

	void FixedUpdate() 
	{
		Follow ();
	}

	public void Follow() 
	{
		if (player) 
		{
			Vector3 point = playerCam.WorldToViewportPoint (player.position);
			Vector3 delta = player.position - playerCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
		}
	}

	public void TriggerShake(float shakeMag, float dampSpeed, float shakeDur) 
	{
		shakeMagnitude = shakeMag;
		dampingSpeed = dampSpeed;
		shakeDuration = shakeDur;
		initialPosition = transform.localPosition;
		canShake = true;
	}

	public void ChangeSize(float size, float time) 
	{
		resetting = false;
		newSize = size;
		sizeTime = time;
		changing = true;
	}

	public void ResetSize() 
	{
		changing = false;
		resetting = true;
	}

	public void BossRoom(Transform boss, float size) {
		player = boss;
		defaultCamSize = size;
		ChangeSize (size, 0.5f);
	}
		
	public void BossDead() {
		bossDeath = true;
	}

	public float GetSize() {
		return defaultCamSize;
	}
}
