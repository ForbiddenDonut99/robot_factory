using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public static bool animationDone = false;
	public Transform target;

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
		transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 1.5f);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 1f);
	}
}
