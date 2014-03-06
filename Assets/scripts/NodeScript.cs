using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {

	//Super nodes are the one-per room nodes designed to navigate the guard
	//from one room to the next.
	public bool isSuper = false;
	public bool isOff = false;

	void Start(){
		gameObject.tag = "Node";
	}

	void onTriggerEnter (Collider other){
		//if the guard passes through, makes node ineligible for selection.
		//Also tells the guard this was the last node selected.
		if(other.GetComponent<GuardPatrol>() != null){
			isOff = true;
			other.GetComponent<GuardPatrol>().lastNode = gameObject;
		}
	}
	void onTriggerExit (Collider other){
		//makes node eligible again once guard leaves it.
		if(other.GetComponent<GuardPatrol>() != null) isOff = false;
	}

}
