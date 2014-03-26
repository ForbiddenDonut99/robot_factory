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
	public AudioClip sound;

	public const int POWERUPTYPEWHEEL = 0;
	public const int POWERUPTYPESTUNGUN = 1;
	public const int POWERUPTYPESCOPE = 2;
	public const int POWERUPTYPECOMPASS = 3;
	public const int POWERUPTYPESPRING = 4;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			audio.PlayOneShot (sound, OptionsMenu.sfx);
			Destroy(gameObject, sound.length);
			other.transform.GetComponent<RobotController>().GetPowerUp(gameObject);
		}
	}
}
