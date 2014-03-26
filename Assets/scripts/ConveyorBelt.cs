using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

	public float beltSpeed = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other) {
		if(other.tag == "Player"){
			Transform playerTransform = other.transform;
			playerTransform.position = new Vector3(playerTransform.position.x,playerTransform.position.y,playerTransform.position.z + Time.deltaTime * beltSpeed);
		}
	}
}
