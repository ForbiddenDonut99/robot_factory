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
			other.transform.position = new Vector3(other.transform.position.x,other.transform.position.y,other.transform.position.z + Time.deltaTime * beltSpeed * .6f);
		} else{
			other.transform.position = new Vector3(other.transform.position.x,other.transform.position.y,other.transform.position.z + Time.deltaTime * beltSpeed);
		}
	}
}
