using UnityEngine;
using System.Collections;

public class TestPoints : MonoBehaviour {

	private MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		meshFilter = (MeshFilter) this.gameObject.GetComponent("MeshFilter");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		if (meshFilter != null) {
			foreach (Vector3 point in meshFilter.mesh.vertices) {
				Gizmos.DrawSphere((this.transform.rotation * point) + this.transform.position, 0.03f);
			}
		}
	}
}