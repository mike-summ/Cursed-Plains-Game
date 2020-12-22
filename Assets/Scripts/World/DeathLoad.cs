using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLoad : MonoBehaviour {

	public void DeathScene() {
		SceneManager.LoadScene (3);
	}

	public void WinLoad() {
		SceneManager.LoadScene (4);
	}
}
