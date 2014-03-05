using UnityEngine;
using System.Collections;

public class ShellGenerator : MonoBehaviour {

	public Transform RoomA;
	public Transform RoomB;
	public Transform RoomC;
	private bool hasA, hasB, hasC, second, third;

	void Start () {

		//Instantiate First Shell
		int pattern = Random.Range (0,3);
		Debug.Log ("First Room: " + pattern);
		if (pattern == 0){
			Instantiate (RoomA,new Vector3(34,0,0),Quaternion.Euler(new Vector3(-90,-270,0)));
			hasA = true;
		}
		else if (pattern == 1){
			Instantiate (RoomB,new Vector3(34,0,0),Quaternion.Euler(new Vector3(-90,-270,0)));
			hasB = true;
		}
		else{
			Instantiate (RoomC,new Vector3(34,0,0),Quaternion.Euler(new Vector3(-90,-270,0)));
			hasC = true;
		}

		//Instantiate Second Shell
		while(!second){
			pattern = Random.Range (0,3);
			Debug.Log ("Trying out room: " + pattern);
			if (pattern == 0 && !hasA){
				Instantiate (RoomA,new Vector3(0,0,-34),Quaternion.Euler(new Vector3(-90,-180,0)));
				hasA = true;
				second = true;
			}
			else if (pattern == 1 && !hasB){
				Instantiate (RoomB,new Vector3(0,0,-34),Quaternion.Euler(new Vector3(-90,-180,0)));
				hasB = true;
				second = true;
			}
			else if (pattern == 2 && !hasC){
				Instantiate (RoomC,new Vector3(0,0,-34),Quaternion.Euler(new Vector3(-90,-180,0)));
				hasC = true;
				second = true;
			}
			else second = false;

		}

		//Instantiate Third Shell
		while(!third){
			pattern = Random.Range (0,3);
			Debug.Log ("Trying out room: " + pattern);
			if (pattern == 0 && !hasA){
				Instantiate (RoomA,new Vector3(-34,0,0),Quaternion.Euler(new Vector3(-90,270,0)));
				hasA = true;
				third = true;
			}
			else if (pattern == 1 && !hasB){
				Instantiate (RoomB,new Vector3(-34,0,0),Quaternion.Euler(new Vector3(-90,270,0)));
				hasB = true;
				third = true;
			}
			else if (pattern == 2 && !hasC){
				Instantiate (RoomC,new Vector3(-34,0,0),Quaternion.Euler(new Vector3(-90,270,0)));
				hasC = true;
				third = true;
			}
			else third = false;
		}
		Debug.Log ("All Rooms Spawned.");

	

	}
}
