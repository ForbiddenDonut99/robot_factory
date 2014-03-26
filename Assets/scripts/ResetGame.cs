using UnityEngine;
using System.Collections;

public class ResetGame : MonoBehaviour {


	void Update () {

		if (Input.GetKey (KeyCode.R)) {
		Application.LoadLevel("zhan_roomGen");
		}
	}
}
