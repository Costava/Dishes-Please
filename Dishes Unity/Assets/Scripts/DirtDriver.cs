using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DirtDriver : MonoBehaviour {

	public MeshFilter meshFilter;

	public GameObject dirtSpeck;

	public GameObject dirtGroup;

	public int dirtAmount = 30;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateDirt(List<Vector3> pointList) {
		foreach (Vector3 point in pointList) {
			GameObject d = (GameObject) Instantiate (dirtSpeck, point, new Quaternion());

			// dirt is spawned under dirt group
			if (this.dirtGroup != null) {
				d.transform.parent = this.dirtGroup.transform;
			}
			else {
				d.transform.parent = this.transform;
			}
		}
	}

	public List<Vector3> GenerateDirtPoints(int dirtQuantity) {
		if (dirtQuantity <= 0) {
			return new List<Vector3>();
		}
		else {
			if (meshFilter != null) {
				int numVertices = meshFilter.mesh.vertices.Length;

				// if you want more dirt than vertices, return all vertices
				if (dirtQuantity >= numVertices) {
					return meshFilter.mesh.vertices.Select(v => meshFilter.transform.TransformPoint(v)).ToList();
				}

				List<Vector3> pointList = new List<Vector3>(dirtQuantity);

				for (int i = 0; i < dirtQuantity; i++) {
					int index = Random.Range(0, numVertices);

					//pointList.Add((this.transform.rotation * meshFilter.mesh.vertices[index]) + this.transform.position);
					pointList.Add(meshFilter.transform.TransformPoint(meshFilter.mesh.vertices[index]));
				}

				return pointList;
			}
			else {
				return new List<Vector3>();
			}
		}
	}
	
//	void OnDrawGizmos() {
//		if (meshFilter != null) {
//			foreach (Vector3 point in meshFilter.mesh.vertices) {
//				Gizmos.DrawSphere((this.transform.rotation * point) + this.transform.position, 0.03f);
//			}
//		}
//	}
}