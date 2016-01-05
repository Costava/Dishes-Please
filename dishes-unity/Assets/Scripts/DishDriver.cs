using UnityEngine;
using System.Collections;

public class DishDriver : MonoBehaviour {

	DishManager dishManager;

	private int targetPathIndex = 0;
	public int TargetPathIndex {
		get {
			return targetPathIndex;
		}

		set {
			if (dishManager == null) {
				init ();
			}

			if (dishManager != null) {
				targetPathIndex = Mathf.Clamp(value, 0, dishManager.dishPath.Length);
			}
			else {
				Debug.LogWarning("dishManager is null in DishDriver. targetPathIndex reset.");
				targetPathIndex = 0;
			}
		}
	}

	// Use this for initialization
	void Start () {
		init ();
	}

	void init() {
		GameObject manager = GameObject.FindGameObjectWithTag("Manager");
		
		if (manager != null) {
			dishManager = manager.GetComponent<DishManager>();
		}

		// Freeze the dish
		GetComponent<Rigidbody>().isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < 0) {
			Destroy(this.gameObject);
		}
	}

	public void MoveDish() {
		if (dishManager != null) {
			transform.position = Vector3.MoveTowards(transform.position, dishManager.dishPath[TargetPathIndex].transform.position, 5f * Time.deltaTime);
			
			// if we reached second position, starting going to third
			if (Mathf.Approximately(Vector3.Distance(dishManager.dishPath[1].transform.position, this.gameObject.transform.position), 0f)) {
				TargetPathIndex = 2;
			}
		}
	}

	public void Fling() {
		// Unfreeze the dish
		//rigidbody.constraints = RigidbodyConstraints.None;
		GetComponent<Rigidbody>().isKinematic = false;

		// and launch it
		GetComponent<Rigidbody>().AddForce((dishManager.dishPath[3].transform.position - transform.position) * 170f);
	}
}
