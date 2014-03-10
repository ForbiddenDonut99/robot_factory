using UnityEngine;
using System.Collections;

public class TrialPatrol : MonoBehaviour {

	public string state = "";
	public GameObject lastNode;
	public GameObject targetNode;
	public int stepsInRoom = 0;
	public float nodeDistance = 20f;
	public float baseSpeed = 1f;
	float moveSpeed;
	CharacterController guardController;
	// Use this for initialization
	void Start () {
		state = "FindNode";
		moveSpeed = baseSpeed;
		guardController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

		if(targetNode != null && lastNode != null){
			if(targetNode.GetComponent<NodeScript>().canReset && lastNode.GetComponent<NodeScript>().canReset){
				stepsInRoom = 0;
			}
		}
				//TODO: Precede this with boolean canSeePlayer that looks for the player. If true,
		//state = "ChasePlayer"
		switch (state){
		case ("FindNode"):
			targetNode = Utility.selectNode(gameObject, nodeDistance, stepsInRoom, lastNode);
			state = "MoveToNode";
			stepsInRoom++;
			break;

		case ("MoveToNode"):
			if(targetNode == null){
				state = "FindNode";
			}
			else{
				guardController.Move (Vector3.Normalize(targetNode.transform.position - transform.position)*Time.deltaTime*moveSpeed);
			}
			break;

		case ("ChasePlayer"):
			//TODO: Chase the player, switch to "FindNode" if canSeePlayer is false.
			break;
		
		}
	}
}
