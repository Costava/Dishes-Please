using UnityEngine;
using System.Collections;

public class RotateDish : MonoBehaviour {

	GameObject manager;
	MyGUI myGUI;

	public float speed = 500f;

	void Start() {
		manager = GameObject.FindGameObjectWithTag("Manager");
	}

	void Update() {
		if (Input.GetKey (KeyCode.Space)) {
			if (manager != null) {
				myGUI = manager.GetComponent<MyGUI>();

				if (myGUI != null) {
					speed = myGUI.dishRotationSensitivitySlider;
				}
			}
			else {
				manager = GameObject.FindGameObjectWithTag("Manager");
			}

			//transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
			transform.rotation = Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed) * this.gameObject.transform.rotation;
		}
	}

}