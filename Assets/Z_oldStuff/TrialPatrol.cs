using UnityEngine;
using System.Collections;

public class TrialPatrol : MonoBehaviour {

	public string state = "";
	public GameObject lastNode;
	public GameObject targetNode;
	public GameObject player;
	public int stepsInRoom = 0;
	public float nodeDistance = 20f;
	public float baseSpeed = 1f;
	public float rotation = 1.0f;
	float moveSpeed;
	CharacterController guardController;
	// Use this for initialization
	void Start () {
		state = "FindNode";
		moveSpeed = baseSpeed;
		guardController = GetComponent<CharacterController>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		//Check for player detection.
		if(Sense.player(gameObject, player))state = "ChasePlayer";

		//Reset the StepsInRoom counter if you go from reset node to reset node.
		if(targetNode != null && lastNode != null){
			if(targetNode.GetComponent<NodeScript>().canReset && lastNode.GetComponent<NodeScript>().canReset){
				stepsInRoom = 0;
			}
		}

		//Main Switch Control
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
				Vector3 finalFacing = (targetNode.GetComponent<Transform>().position - transform.position).normalized;
				transform.forward = Vector3.Lerp (transform.forward, finalFacing, Time.deltaTime*rotation);
				guardController.Move (Vector3.Normalize(targetNode.transform.position - transform.position)*Time.deltaTime*moveSpeed);
			}
			break;

		case ("ChasePlayer"):
			if (!Sense.player(gameObject, player))state = "FindNode";
			else{
				guardController.Move (Vector3.Normalize(player.transform.position - transform.position)*Time.deltaTime*moveSpeed*1.5f);
				Vector3 finalFacing = (player.GetComponent<Transform>().position - transform.position).normalized;
				transform.forward = Vector3.Lerp (transform.forward, finalFacing, Time.deltaTime*rotation);
			}
			break;
		
		
		}
	}
}
