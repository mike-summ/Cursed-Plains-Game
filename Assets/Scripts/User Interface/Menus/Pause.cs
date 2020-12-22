using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

	public GameObject pauseMenu;

	private bool paused = false;

	void Start() {
		pauseMenu.SetActive (false);
	}

	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			if (paused) {
				Resume ();
			} else {
				PauseMenu ();			
			}
		}
	}

	void PauseMenu() {
		paused = true;
		Time.timeScale = 0;
		pauseMenu.SetActive (true);
	}

	public void Resume() {
		paused = false;
		Time.timeScale = 1;
		pauseMenu.SetActive (false);
	}

	public void Exit() {
		// Go to menu screen
		Time.timeScale = 1;
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}
}
