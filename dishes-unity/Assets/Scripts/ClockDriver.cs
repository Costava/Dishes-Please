using UnityEngine;
using System.Collections;

public class ClockDriver : MonoBehaviour {

	float hour = 0f;
	float minute = 0f;

	float stopHour = 12f;
	float stopMinute = 60f;

	float minutesPerSecond = 90f;

	ClockController clockController;

	bool intervalSet = false;

	// Use this for initialization
	void Start () {
		clockController = this.GetComponent<ClockController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (intervalSet && TimeAttackStatus == TimeAttackState.started) {
			SpinClock();
		}
	}

	// cannot turn past start point
	public void SetInterval(float startHour, float startMinute, float endHour, float endMinute, float realTimeSeconds) {
		stopHour = endHour;
		stopMinute = endMinute;

		float hourChange = endHour - startHour;
		float minuteChange = endMinute - startMinute;

		float intervalLengthInMinutes = (hourChange * 60f) + minuteChange;
		minutesPerSecond = intervalLengthInMinutes / realTimeSeconds;

		if (clockController != null) {
			hour = startHour;
			minute = startMinute;

			clockController.SetHour(hour);
			clockController.SetMinute(minute);
			intervalSet = true;
		}
	}

	public void SpinClock() {
		float minutesToAdd = minutesPerSecond * Time.deltaTime;
		minute += minutesToAdd;
		hour += minutesToAdd / 60f;

		//Debug.Log(hour + " : " + minute);
		//Debug.Log(hour + " : " + minute + " Stop Time | " + stopHour + " : " + stopMinute);

		if (TimeAttackStatus == TimeAttackState.started) {
			if (hour >= stopHour && minute >= stopMinute) {
				TimeAttackStatus = TimeAttackState.ended;
				intervalSet = false;
			}
		}

		if (minute > 60) {
			minute %= 60f;
		}
		
		if (hour > 12) {
			hour %= 12;
		}
		
		if (clockController != null) {
			clockController.SetHour(hour);
			clockController.SetMinute(minute);
		}
	}

	public delegate void TimeAttackEventHandler(MonoBehaviour sender);
	public TimeAttackEventHandler OnTimeAttackStarted = delegate { };
	public TimeAttackEventHandler OnTimeAttackEnded = delegate { };
	
	public enum TimeAttackState {
		notStarted, started, paused, ended
	}
	
	private TimeAttackState _timeAttackStatus = TimeAttackState.notStarted;
	public TimeAttackState TimeAttackStatus
	{
		get {
			return this._timeAttackStatus;
		}
		set {
			// If we are changing from not started to started
			if (this._timeAttackStatus != TimeAttackState.started && value == TimeAttackState.started) {
				Debug.Log ("Time Attack Started");
				this.OnTimeAttackStarted(this);
			}

			// if we are changing from started to ended
			if (this._timeAttackStatus == TimeAttackState.started && value == TimeAttackState.ended) {
				Debug.Log ("Time Attack Ended");
				this.OnTimeAttackEnded(this);
			}
			
			this._timeAttackStatus = value;
		}
	}
}
