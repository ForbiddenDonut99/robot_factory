using UnityEngine;
using System.Collections;

public class AILevel : MonoBehaviour {

	public GUIText startText;

	void Start () {

		StartCoroutine ("startTextCor");
		}

	void Update () {
	
		if (Input.GetKey (KeyCode.R)) {

			Application.LoadLevel("PopulationWorkspace");
		}
	}

	IEnumerator startTextCor() {
		yield return new WaitForSeconds (5);
		startText.enabled = false;

	}


}
