using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public int cleanDishes = 0;
	public GameObject clock;
	public ClockDriver clockDriver;

	// Use this for initialization
	void Start () {
		if (clock != null) {
			clockDriver = clock.GetComponent<ClockDriver>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartTimeAttack() {
		cleanDishes = 0;

		if (clockDriver != null) {
			clockDriver.SetInterval(0f, 0f, 6f, 0f, 3f * 60f);
			clockDriver.TimeAttackStatus = ClockDriver.TimeAttackState.started;
		}
	}

	public void DishCleaned() {
		if (clockDriver != null) {
			if (clockDriver.TimeAttackStatus == ClockDriver.TimeAttackState.started) {
				Debug.Log ("Dish Cleaned");
				cleanDishes += 1;
			}
		}
	}
}
