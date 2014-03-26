using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public static bool animationDone = false;
	public Transform target;
	float moveSpeed = 0.1f;

	// Use this for initialization
	void Start () {
		if (animationDone){
			transform.position = target.position;
			transform.rotation = target.rotation;
		} else{
			animationDone = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		moveSpeed += Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 1.5f * moveSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 1f * moveSpeed);
	}
}
