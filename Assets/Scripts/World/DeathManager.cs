using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour {

	public float deathDuration = 2f;

	void Update () {
		if (deathDuration <= 0) {
			SceneManager.LoadScene (2);
		} else {
			deathDuration -= Time.deltaTime;
		}
	}
}
