using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	void Start() {
		AudioBase musicAudioBase = GetComponent<AudioBase>();

		if (musicAudioBase != null) {
			musicAudioBase.Play();
		}
	}

//	// Use this for initialization
//	void Start () {
//		Screen.lockCursor = true;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//		// Take care of mouse lock
//		if (Input.GetKeyDown("escape"))
//			Screen.lockCursor = false;
//		else if (Input.GetMouseButtonDown(0))
//			Screen.lockCursor = true;
//	}
}
