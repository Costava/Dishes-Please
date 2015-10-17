using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour {

	public GameObject clock;
	ClockDriver clockDriver;

	ScoreManager scoreManager;
	AudioManager audioManager;

	public Camera camera;
	HeadMove headMove;

	bool showMenu = true;

	int padding = 10;
	int buttonWidth = 230;
	int buttonHeight = 20;

	public float musicSlider = 1f;
	public float sfxSlider = 1f;
	public float headMoveSensitivitySlider = 15f;
	public float dishRotationSensitivitySlider = 500f;

	GUIStyle rightStyle;
	int rightWidth = 340;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = false;

		scoreManager = GetComponent<ScoreManager>();
		audioManager = GetComponent<AudioManager>();
		
		if (clock != null) {
			clockDriver = clock.GetComponent<ClockDriver>();
			
			if (clockDriver != null) {
				clockDriver.OnTimeAttackStarted += (sender) =>  {
					showMenu = false;
					Screen.lockCursor = true;
				};

				clockDriver.OnTimeAttackEnded += (sender) =>  {
					showMenu = true;
					Screen.lockCursor = false;
				};
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			showMenu = !showMenu;
			Screen.lockCursor = !Screen.lockCursor;
		}

		if (showMenu == false) {
			Screen.lockCursor = true;
		}

		// Update music and sound effects levels if they changed
		if (audioManager != null) {
			if (audioManager.GetMasterVolume(AudioBase.AudioType.Music) != musicSlider) {
				audioManager.SetMasterVolume(AudioBase.AudioType.Music, musicSlider);
			}

			if (audioManager.GetMasterVolume(AudioBase.AudioType.SoundEffect) != sfxSlider) {
				audioManager.SetMasterVolume(AudioBase.AudioType.SoundEffect, sfxSlider);
			}
		}

		// Update head moving mouse sensitivity if it has changed
		if (camera != null) {
			headMove = camera.GetComponent<HeadMove>();

			if (headMove != null) {
				if (headMove.sensitivityX != headMoveSensitivitySlider || headMove.sensitivityY != headMoveSensitivitySlider) {
					headMove.sensitivityX = headMoveSensitivitySlider;
					headMove.sensitivityY = headMoveSensitivitySlider;
				}
			}
		}
	}

	void OnGUI() {
		rightStyle = new GUIStyle(GUI.skin.button);
		rightStyle.alignment = TextAnchor.UpperLeft;

		if (showMenu) {
			// GUI on right side
			GUI.Button(new Rect(Screen.width - padding - rightWidth, padding, rightWidth, buttonHeight), "Dishes, Please");

			GUI.Button(new Rect(Screen.width - padding - rightWidth, 2 * padding + buttonHeight, rightWidth, 9 * buttonHeight), new GUIContent("Controls\n\nClick to spray and wash the dishes.\nHold space to rotate the dish.\nPress \"c\" when the dish is clean to get the next one.\n\nTime Attack\n\nClean as many dishes as you can in 3 minutes.\nYou will only receive points for completely clean dishes\nWatch the clock - your shift is from 12:00 to 6:00."), rightStyle);

			GUI.Button(new Rect(Screen.width - padding - rightWidth, 3 * padding + 10 * buttonHeight, rightWidth, 10 * buttonHeight), new GUIContent("Credits\n\nGame by Costava. View source at \ngithub.com/Costava/Dishes-Please\n\nSpecial Thanks\nEric Eastwood (ericeastwood.com): Radius\nBossLevelVGM: Music\ncemkalyoncu: Kitchen\nTech Knight\nvipergames\nSamuel Moxham\nAszura"), rightStyle);



			// Quit button
			if (GUI.Button(new Rect(padding, padding, buttonWidth, buttonHeight), "Quit")) {
				Application.Quit();
			}



			// Music slider row
			GUI.Button(new Rect(padding, 2 * padding + buttonHeight, 0.5f * buttonWidth, buttonHeight), "Music");
			musicSlider = GUI.HorizontalSlider(new Rect(2 * padding + 0.5f * buttonWidth, 2 * padding + buttonHeight, 0.5f * buttonWidth, buttonHeight), musicSlider, 0f, 1f);

			// Sound effects slider row
			GUI.Button(new Rect(padding, 3 * padding + 2 * buttonHeight, 0.5f * buttonWidth, buttonHeight), "Sound Effects");
			sfxSlider = GUI.HorizontalSlider(new Rect(2 * padding + 0.5f * buttonWidth, 3 * padding + 2 * buttonHeight, 0.5f * buttonWidth, buttonHeight), sfxSlider, 0f, 1f);

			// Mouse sensitivy row
			GUI.Button(new Rect(padding, 4 * padding + 3 * buttonHeight, 0.5f * buttonWidth, buttonHeight), "Mouse Sensitivity");
			headMoveSensitivitySlider = GUI.HorizontalSlider(new Rect(2 * padding + 0.5f * buttonWidth, 4 * padding + 3 * buttonHeight, 0.5f * buttonWidth, buttonHeight), headMoveSensitivitySlider, 0.1f, 30f);

			// Dish rotation sensitivity row
			GUI.Button(new Rect(padding, 5 * padding + 4 * buttonHeight, 0.5f * buttonWidth, buttonHeight), "Rotate Sensitivity");
			dishRotationSensitivitySlider = GUI.HorizontalSlider(new Rect(2 * padding + 0.5f * buttonWidth, 5 * padding + 4 * buttonHeight, 0.5f * buttonWidth, buttonHeight), dishRotationSensitivitySlider, 1f, 1000f);



			// Resume button
			if (GUI.Button(new Rect(padding, 6 * padding + 5 * buttonHeight, buttonWidth, buttonHeight), "Resume")) {
				showMenu = false;
				Screen.lockCursor = true;
			}



			// Start Time Attack button
			if (clockDriver != null) {
				if (clockDriver.TimeAttackStatus != ClockDriver.TimeAttackState.started && GUI.Button(new Rect(padding, 8 * padding + 7 * buttonHeight, buttonWidth, buttonHeight), "Start Time Attack")) {
					Debug.Log("Start button pressed");

					showMenu = false;
					Screen.lockCursor = true;

					if (scoreManager != null) {
						Debug.Log("Start Time Attack");

						scoreManager.StartTimeAttack();
					}
				}
			}
		}

		if (scoreManager != null) {
			// Score button
			GUI.Button(new Rect(padding, 10 * padding + 9 * buttonHeight, buttonWidth, buttonHeight), "Score: " + scoreManager.cleanDishes);
		}
	}
}
