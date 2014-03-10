using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {

	//Super nodes are the one-per room nodes designed to navigate the guard
	//from one room to the next.
	public bool isSuper = false;
	public bool canReset = false;
	public bool isOff = false;

	public GameObject next;

	void Start(){
		gameObject.tag = "Node";

	}

	void OnTriggerEnter (Collider other){
		//if the guard passes through, makes node ineligible for selection.
		//Also tells the guard this was the last node selected.
		if(other.GetComponent<GuardPatrol>() != null && !isOff){
			isOff = true;
			//Debug.Log (gameObject + "is off!");
			if(other.GetComponent<GuardPatrol>().targetNode == gameObject){
				other.GetComponent<GuardPatrol>().state = "FindNode";
				//other.GetComponent<GuardPatrol>().targetNode = next;
			}
		}
	}
	void OnTriggerExit (Collider other){
		//makes node eligible again once guard leaves it.
		if(other.GetComponent<GuardPatrol>() != null && isOff){
			isOff = false;
			//Debug.Log(gameObject + "is on!");
			other.GetComponent<GuardPatrol>().lastNode = gameObject;
		}
	}

}
