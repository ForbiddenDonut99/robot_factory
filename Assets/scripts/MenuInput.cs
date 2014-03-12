using UnityEngine;
using System.Collections;

public class MenuInput : MonoBehaviour {

	public KeyCode buttonToPush = KeyCode.Space;
	public string nextLevel = "TymkoGuard";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (buttonToPush)) {

			Application.LoadLevel(nextLevel);
		}
	
	}
}
