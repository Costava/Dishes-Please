using UnityEngine;
using System.Collections;
using System.Linq;

public class SprayDriver : MonoBehaviour {

	public Camera camera;
	public GameObject nozzlePoint;
	public GameObject waterFountain;

	public float sprayRadius = 0.2f;
	public float sprayRange = 10f;

	public AudioBase audioBase;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			if (audioBase != null) {
				//Debug.Log("Play");
				audioBase.Play();
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			if (audioBase != null) {
				audioBase.Stop();
			}
		}


		// if left click
		if (Input.GetMouseButton(0)) {
			//Debug.Log ("Click");

			//waterFountain.renderer.enabled = true;
			waterFountain.GetComponent<ParticleEmitter>().emit = true;

			Vector3? cameraHit = GetFirstHit(camera.transform.position, camera.transform.forward, sprayRange);
			
			if (cameraHit != null) {
				//Debug.Log ("Hit");

				transform.LookAt((Vector3) cameraHit, Vector3.up);

				Vector3? sprayerHit = GetFirstHit(nozzlePoint.transform.position, (Vector3) cameraHit - nozzlePoint.transform.position, sprayRange);

				if (sprayerHit != null) {
					Collider[] overlapSphereColliders = Physics.OverlapSphere((Vector3) sprayerHit, sprayRadius);

					foreach (Collider collider in overlapSphereColliders) {
						if (collider.GetComponent<Dirt>() != null) {
							if (Physics.RaycastAll(nozzlePoint.transform.position, collider.transform.position - nozzlePoint.transform.position, sprayRange).Select(r => r.collider).ToList().Contains(collider)) {
								Destroy(collider.gameObject);
							}
						}
					}
				}
			}
			else {
				if (camera != null) {
					transform.LookAt(camera.transform.forward * 12 + camera.transform.position, Vector3.up);
				}
			}
		}
		else {
			//waterFountain.renderer.enabled = false;
			waterFountain.GetComponent<ParticleEmitter>().emit = false;
		}
	}

	Vector3? GetFirstHit(Vector3 origin, Vector3 direction, float range) {
		RaycastHit[] hits = Physics.RaycastAll(origin, direction, range).OrderBy(i => i.distance).ToArray();

		if (hits.Length > 0) {
			return hits[0].point;
		}
		else {
			return null;
		}
	}
}
