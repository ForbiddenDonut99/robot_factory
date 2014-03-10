using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
	/*
	*  0 : wheels, speed +
	*  1 : flashlight, enable flashlight
	*  2 : stungun, ammo+
	*/
	public int PowerUpType;
	public float PowerUpValue;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			other.transform.GetComponent<RobotController>().PowerUp(PowerUpType, PowerUpValue);
			Destroy(gameObject);
		}
	}
}
