using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldInfo : MonoBehaviour
{

	PlayerController pc;

	public Vector2 checkpoint;
	public int noOfJumps;
	public bool primal;
	public List<string> bossNames;
	public bool princeKey = false;
	public float time;
	public float bestTime;

	private float aTime = 1;
	private float bTime = 0;

	void Start ()
	{
		DontDestroyOnLoad (gameObject);

		if (SceneManager.GetActiveScene ().buildIndex == 2) {
			for (int i = 0; i < bossNames.Count; i++) {
				Destroy (GameObject.Find (bossNames [i]));
			}
		}
	}

	void Update ()
	{
		if (SceneManager.GetActiveScene ().buildIndex == 2) {
			try {
				pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

				checkpoint = pc.checkpoint;
				noOfJumps = pc.amountOfJumps;
				primal = pc.gameObject.GetComponent<Attack> ().primalUnlocked;
			} catch {
				Debug.Log ("Most likely dead");		
			}
		}

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			if (bTime >= aTime) {
				bTime = 0;
				time += 1;
			} else {
				bTime += Time.deltaTime;
			}
		}
	}

	public void Finish ()
	{

		if (time <= bestTime || bestTime == 0) {
			Debug.Log (time);
			bestTime = time;
		}
		time = 0;
		checkpoint = new Vector2 (-26, -10);
		bossNames.Clear ();
		princeKey = false;
	}
}
