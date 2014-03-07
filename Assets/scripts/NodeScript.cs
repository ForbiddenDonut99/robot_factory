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

	void OnTriggerEnter (Collider other){
		//if the guard passes through, makes node ineligible for selection.
		//Also tells the guard this was the last node selected.
		Debug.Log("ENTER");
		if(other.GetComponent<GuardPatrol>() != null){
			isOff = true;
			GuardPatrol guard = other.GetComponent<GuardPatrol>();
			guard.stepsInRoom ++;
			GameObject nextNode = Utility.selectNode(this.gameObject, 50f, guard.stepsInRoom, this.gameObject);
			guard.walkTowards(nextNode.transform);
			other.GetComponent<GuardPatrol>().lastNode = gameObject;
		}
	}
	void OnTriggerExit (Collider other){
		//makes node eligible again once guard leaves it.
		if(other.GetComponent<GuardPatrol>() != null) isOff = false;
	}

}
