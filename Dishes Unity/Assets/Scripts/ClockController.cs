using UnityEngine;
using System.Collections;

public class ClockController : MonoBehaviour {

	public Transform hourBone;
	public Transform minuteBone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetHour(float hour) {
		hour %= 12f;
		float rotationProportion = hour / 12f;

		hourBone.transform.localEulerAngles = new Vector3(0f, 0f, -1f * rotationProportion * 360f);
	}

	public void SetMinute(float minute) {
		minute %= 60f;
		float rotationProportion = minute / 60f;

		minuteBone.transform.localEulerAngles = new Vector3(0f, 0f, -1f * rotationProportion * 360f);
	}
}
