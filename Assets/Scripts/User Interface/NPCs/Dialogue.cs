using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	public GameObject txtGO;
	public string[] dialogueText;
	public string interactBtn = "Interact";

	private int index = 0;
	private bool complete = false;
	private GameObject txtParent;

	void Start() {
		txtParent = txtGO.transform.parent.gameObject;
		txtGO.SetActive (false);
	}

	void Update() {
		if (!isFinished ()) {
			Text txt = txtGO.GetComponent<Text> ();
			txt.text = dialogueText [index];

			if (Input.GetButtonDown (interactBtn)) {
				index++;
			}
		} else {
			Stop ();
		}
	}

	bool isFinished() {
		if ((index + 1) > dialogueText.Length || complete) {
			return true;
		} else {
			return false;
		}
	}

	public void Stop() {
		complete = true;
		txtGO.SetActive (false);
	}

	public void StartDialogue(string[] dialogue) {
		txtGO.SetActive (true);
		dialogueText = dialogue;
		index = 0;
		complete = false;
	}
}
