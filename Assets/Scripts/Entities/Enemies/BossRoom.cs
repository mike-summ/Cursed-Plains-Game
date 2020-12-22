using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour {

	CameraController cc;
	public GameObject boss;
	public GameObject bossWalls;

	void Start() {
		cc = Camera.main.GetComponent<CameraController> ();
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag.Equals ("Player")) {
            if (boss != null)
            {
                boss.SendMessage("EnterBossRoom");
                bossWalls.SetActive(true);
                cc.BossRoom(transform, 15f);
            }
            else {
                bossWalls.SetActive(false);
            }
		}
	}

	public void PlayerDeath() {
		cc.BossDead ();
		bossWalls.SetActive (false);
		boss.SendMessage ("Reset");
	}
}
