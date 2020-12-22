using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public GameObject[] levels;
	public int currentLevel = 0;

	public void UpdateLevel(string levelName) {

		// find the current level
		// deactivate everything
		for (int i = 0; i < levels.Length; i++) {
			if (levels [i].name.Equals (levelName)) {
				currentLevel = i;
			}
			levels [i].SetActive (false);
		}

		// set the current level and surrounding levels to active
		levels [currentLevel].SetActive(true);

		int prevLevel = currentLevel - 1;
		int nxtLevel = currentLevel + 1;

		if (prevLevel >= 0) {
			levels [prevLevel].SetActive (true);
		}

		if (nxtLevel <= (levels.Length - 1)) {
			levels [nxtLevel].SetActive (true);
		}
	}
}
