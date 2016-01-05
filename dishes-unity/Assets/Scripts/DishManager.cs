using UnityEngine;
using System.Collections;

public class DishManager : MonoBehaviour {

	public GameObject[] dishChoices;

	public GameObject[] dishPath;

	public GameObject dishPrefab;

	public GameObject currentDish;

	// Use this for initialization
	void Start () {
		GameObject clock = GameObject.FindGameObjectWithTag("Clock");

		if (clock != null) {
			ClockDriver clockDriver = clock.GetComponent<ClockDriver>();

			if (clockDriver != null) {
				clockDriver.OnTimeAttackStarted += (sender) =>  {
					NextDish();
				};

				clockDriver.OnTimeAttackEnded += (sender) =>  {
					if (currentDish != null) {
						DishDriver dishDiver = currentDish.GetComponent<DishDriver>();

						if (dishDiver != null) {
							dishDiver.Fling();
							currentDish = null;
						}
					}
				};
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (currentDish != null) {
			DishDriver dishDriver = currentDish.GetComponent<DishDriver>();

			if (dishDriver != null) {
				dishDriver.MoveDish();

				if (Input.GetKeyDown(KeyCode.C) && dishDriver.TargetPathIndex == 2) {
					DirtDriver dirtDriver = currentDish.GetComponent<DirtDriver>();

					if (dirtDriver != null) {
						if (dirtDriver.dirtGroup.transform.childCount == 0) {
							ScoreManager scoreManager = GetComponent<ScoreManager>();

							if (scoreManager != null) {
								scoreManager.DishCleaned();
							}
						}
					}

					dishDriver.Fling();
					currentDish = null;
					NextDish();
				}
			}
		}
	}

	void NextDish() {
		if (currentDish == null && dishChoices.Length > 0 && dishPath.Length > 0) {
			int dishNumber = Random.Range(0, dishChoices.Length);

			// Make a dish wrapper
			GameObject dish = (GameObject) Instantiate(dishPrefab, dishPath[0].transform.position, Quaternion.identity);

			// Make dish and put in wrapper
			GameObject dishModel = (GameObject) Instantiate(dishChoices[dishNumber], dishPath[0].transform.position, dishChoices[dishNumber].transform.rotation);
			dishModel.transform.parent = dish.transform;
			Destroy(dishModel.GetComponent<Rigidbody>());

			// Put dirt on model
			DirtDriver dirtDriver = dish.GetComponent<DirtDriver>();
			dirtDriver.meshFilter = dishModel.GetComponent<MeshFilter>();
			dirtDriver.CreateDirt(dirtDriver.GenerateDirtPoints(dirtDriver.dirtAmount));

			var dishDriver = dish.GetComponent<DishDriver>();
			if (dishDriver != null) {
				dishDriver.TargetPathIndex = 1;
			}

			currentDish = dish;
		}
	}
}
