using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour {

	PlayerController pc;
	public GameObject[] healthUI;
	public Sprite healthFull;
	public Sprite healthEmpty;
	public GameObject rageBar;

	private int playerHealth;
	private int prevHealth;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();	
	}
	
	// Update is called once per frame
	void Update () {
		playerHealth = pc.health;

		if (prevHealth != playerHealth) {
			UpdateHealth ();
		}

		prevHealth = playerHealth;

		rageBar.GetComponent<Slider> ().value = Mathf.Lerp (rageBar.GetComponent<Slider> ().value, pc.rage, 0.03f); 
	}

	void UpdateHealth() {
		for (int i = 0; i < 5; i++) {
			healthUI [i].GetComponent<Image> ().sprite = healthEmpty;
		}
		for (int i = 0; i < playerHealth; i++) {
			healthUI [i].GetComponent<Image> ().sprite = healthFull;
		}
	}
}
