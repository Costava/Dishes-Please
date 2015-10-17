using UnityEngine;
using System.Collections;

public class HeadMove : MonoBehaviour {

	public Transform headTransform; // Used to rotate the head
	public Transform bodyTransform; // Used to rotate the body when head rotation limit reached

	public enum RotationAxes { 
		MouseXAndY, MouseX, MouseY 
	}
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15f;
	public float sensitivityY = 15f;
	
	public float rotationMinimumX = -360f;
	public float rotationMaximumX = 360f;
	
	public float rotationMinimumY = -60f;
	public float rotationMaximumY = 60f;



	float rotationX = 0f;
	float rotationY = 0f;

	Quaternion headOriginalRotation;


	// Use this for initialization
	void Start () {
		if(this.headTransform)
		{
			this.headOriginalRotation = this.headTransform.rotation;
		}
		else
			Debug.LogError("Missing headTransform in HeaderMove script");
	}
	
	// Has to be LateUpdate because of Mecanim
	void LateUpdate () {
		// Screen lock is commented out to avoid game breaking behavior:
		//  when you spray, the camera cannot be moved until you
		//  bring up the menu and then close it
		if(/*Screen.lockCursor && */!Input.GetKey(KeyCode.Space))
		{
			Debug.Log ("hm lateupdate");
			this.rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			this.rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

			float excessXRotation = this.ClampExcess(this.rotationX, this.rotationMinimumX, this.rotationMaximumX);

			if (this.bodyTransform != null) {
				// Rotate the body when we reach the rotation limits
				if (axes == RotationAxes.MouseXAndY || axes == RotationAxes.MouseX)
					this.bodyTransform.localEulerAngles = new Vector3(this.bodyTransform.localEulerAngles.x, this.bodyTransform.localEulerAngles.y + excessXRotation, this.bodyTransform.localEulerAngles.z);
			}

			this.rotationX = Mathf.Clamp(this.rotationX, this.rotationMinimumX, this.rotationMaximumX);
			this.rotationY = Mathf.Clamp(this.rotationY, this.rotationMinimumY, this.rotationMaximumY);
			//Debug.Log(this.rotationX + " " + this.rotationY);
		}

		Quaternion xQuaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis(this.rotationY, -Vector3.right);

		// We want parent rotation
		Quaternion parentRotation = transform.parent != null ? transform.parent.rotation : Quaternion.identity;
		// Move the head
		this.headTransform.rotation = this.headOriginalRotation * parentRotation * xQuaternion * yQuaternion;
	
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;

		//Vector3 afds = this.headTransform.localRotation
		//Gizmos.DrawLine(this.headTransform.position
	}

	
	// Returns the excess from min or max
	float ClampExcess(float value, float min, float max)
	{
		if(value < min)
			return value - min;
		else if(value > max)
			return value - max;
		
		return 0;
	}
}
