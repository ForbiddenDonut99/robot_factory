using UnityEngine;
using System.Collections;

public class TrialPatrol : MonoBehaviour {
	
	public string state = "";
	public GameObject lastNode;
	public GameObject targetNode;
	public GameObject player;
	public int stepsInRoom = 0;
	public float nodeDistance = 25f;
	public float baseSpeed = 5f;
	public float baseRotation = 4.0f;
	float moveSpeed;
	float rotation;
	CharacterController guardController;
	// Use this for initialization
	void Start () {
		state = "FindNode";
		moveSpeed = baseSpeed;
		rotation = baseRotation;
		guardController = GetComponent<CharacterController>();
		player = GameObject.FindGameObjectWithTag("Player");
		
		//Find an initial node to be lastNode
		targetNode = Sense.startNode(gameObject, Sense.nearbyNodes(gameObject, nodeDistance));
		lastNode = Sense.startNode(gameObject, Sense.nearbyNodes(gameObject, nodeDistance));
	}
	
	// Update is called once per frame
	void Update () {
		
		//Check for game over.
		if(Vector3.Distance(transform.position, player.transform.position) < 1.2f && !(stunTime > 0.0f)){
			//Debug.Log("PLAYER CAUGHT");
			player.GetComponent<RobotController>().GameOver();
			return;
		}
		
		//Be stunned.
		if(stunTime > 0.0f){
			// handle stun
			stunTime -= Time.deltaTime;
			if(stunTime <= 0.0f){
				//TODO: recover from stun animation
				transform.rotation = Quaternion.identity;
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
			}
			return;
		}
		//else transform.rotation = Quaternion.Euler();
		
		//Check for player detection.
		if(Sense.player(gameObject, player)){
			state = "ChasePlayer";
			//Debug.Log ("Hunting Player!");
			rotation = 4*baseRotation;
		}
		else rotation = baseRotation;
		
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
				guardController.Move (Vector3.Normalize(targetNode.transform.position - transform.position)*Time.deltaTime*moveSpeed);
				
				Quaternion newRotation = Quaternion.LookRotation(targetNode.transform.position - transform.position);
				newRotation.x = 0f;
				newRotation.z = 0f;
				transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime*rotation);
			}
			break;
			
		case ("ChasePlayer"):
			if (!Sense.player(gameObject, player)){
				state = "FindNode";
				//Debug.Log ("Lost sight of Player.");
			}
			else{
				if(!player.GetComponent<RobotController>().isWin){
					// don't move if game is already won
					guardController.Move (Vector3.Normalize(player.transform.position - transform.position)*Time.deltaTime*moveSpeed*1.5f);
				}
				
				Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
				newRotation.x = 0f;
				newRotation.z = 0f;
				transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime*rotation);
			}
			break;
			
			
		}
	}
	
	float stunTime;
	
	public void stun(float time){
		if(stunTime <= 0.0f){
			//TODO: stun animation
			transform.rotation = Quaternion.LookRotation(-transform.up);
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
		}
		stunTime = time;
	}
}
