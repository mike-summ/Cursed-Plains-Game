using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	// Functions for every button in the main menu.
	public float fadeDuration;
	public float timer;
	public float targetAlpha;
	private Image image;

	public GameObject bgArt;
	public GameObject startText;
	public GameObject startButton;
	public GameObject exitButton;
	public GameObject fadingImage;

	public bool isFading = false;
	public bool leaving = false;

	void Start() {
		timer = fadeDuration;
		startText.SetActive (false);
		fadingImage.SetActive (false);
		image = fadingImage.GetComponent<Image> ();
		targetAlpha = image.color.a;
	}

	void Update() {
		Color curColor = image.color;
		float alphaDiff = Mathf.Abs (curColor.a - targetAlpha);
		if (alphaDiff > 0.0001f) {
			curColor.a = Mathf.Lerp (curColor.a, targetAlpha, fadeDuration * Time.deltaTime);
			image.color = curColor;
		}

		if (isFading) {
			if (timer <= 0) {
				bgArt.SetActive (false);
				startButton.SetActive (false);
				exitButton.SetActive (false);
				startText.SetActive (true);
				isFading = false;
				timer = fadeDuration;
				FadeBack ();
			} else {
				timer -= Time.deltaTime;
			}
		}

		if (leaving) {
			if (timer <= 0) {
				timer = fadeDuration;
				leaving = false;
				SceneManager.LoadScene ("Intro_level", LoadSceneMode.Single);
			} else {
				timer -= Time.deltaTime;
			}
		}
	}

	public void FadeToBlack() {
		// Fade to black and show text
		fadingImage.SetActive(true);
		isFading = true;
		targetAlpha = 1.0f;

	}

	public void FadeBack ()  {
		leaving = true;
		targetAlpha = 0.0f;
	}

	public void StartBtn() {
		// Disable Start and Exit buttons and FadeToBlack()
		FadeToBlack();
	}

	public void ExitBtn() {
		// Exit the application
		Application.Quit();
	}
}
