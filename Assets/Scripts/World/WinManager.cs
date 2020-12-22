using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour {

	public float winDuration = 2f;

	void Update () {
		if (winDuration <= 0) {
			SceneManager.LoadScene (0);
		} else {
			winDuration -= Time.deltaTime;
		}
	}
}
