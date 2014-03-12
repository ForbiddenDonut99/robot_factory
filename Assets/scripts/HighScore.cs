using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	// "Static" keyword tells Unity that this variable is automatically made and global

	static int highScore = 0;
	
	void Update() {
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (2);

		}

		if (Input.GetKeyDown(KeyCode.X)) {

			highScore++;

		}

		GetComponent<TextMesh>().text = highScore.ToString ();

	}


}
