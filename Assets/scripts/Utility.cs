using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	public static float getRange(GameObject self, GameObject target){
		/*Compares and returns the distance between a given target and the calling
	 * instance.*/
		Vector3 position = self.transform.position;
		Vector3 difference = target.transform.position-position;
		float distance = difference.sqrMagnitude;
		return distance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
