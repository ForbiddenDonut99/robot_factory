using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	//This is a temporary script. I'm using it to test out half-finished script snippets.

	private int counter = 0;
	void Start () {

		NodeData[] surroundings;
		surroundings = Sense.nearbyNodes(gameObject, 150f);
		if(surroundings != null){
			foreach(NodeData node in surroundings){
				counter++;
				Debug.Log (node + "" + counter);
			}
		}
		Debug.Log (counter + " nodes found.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
