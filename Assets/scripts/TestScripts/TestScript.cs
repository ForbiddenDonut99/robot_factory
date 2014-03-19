using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	//This is a temporary script. I'm using it to test out half-finished script snippets.
	public float distance = 150f;
	private int counter = 0;
	void Start () {
		Debug.Log(OptionsMenu.sfx);
		/*GameObject[] surroundings;
		surroundings = Sense.nearbyNodes(gameObject, distance);
		if(surroundings != null){
			foreach(GameObject node in surroundings){
				counter++;
				Debug.Log (node + "was found!");
			}
		}
		Debug.Log (counter + " nodes found.");*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
