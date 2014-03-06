using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for list

public class GuardPatrol : MonoBehaviour {
	public GameObject player;
	public float baseSpeed = 1f;
	public float wallBuffer = 2f;
	public int stepsInRoom = 0;
	public GameObject lastNode;
	float moveSpeed;

	CharacterController guardController;

	Vector3 nextDirection = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		guardController = GetComponent<CharacterController>();
		moveSpeed = baseSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: end the game instead
		if(Vector3.Distance(transform.position, player.transform.position) < 1.2f){
			Debug.Log("PLAYER CAUGHT");
			return;
		}

		if(chasePlayer()){
//			Debug.Log("player detected");
			// Player seen. Reset generated direction
			nextDirection = Vector3.zero;
		} else{
			moveSpeed = baseSpeed;
			if (continueRotation()){
//				Debug.Log("rotating");
			} else if (walkStraight()){
//				Debug.Log("no walls");
			} else{
//				Debug.Log("switch direction");
				randomWalk();
			}
		}
	}

	// if player seen, rotate towards player, move forward, and return true
	bool chasePlayer(){
		// Raycast at player
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 20f)){
			// check if the player is in front
			if (hit.transform.tag != "Player"){
				return false;
			}
			if(Vector3.Angle(player.transform.position - transform.position, transform.forward) < 60f){
				// player seen, chase by looking at player and running forward, moving at 1.5x speed
				moveSpeed = baseSpeed * 1.5f;
				Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
				guardController.Move(transform.forward.normalized * moveSpeed * Time.deltaTime);
				return true;
			}
		}
		return false;
	}
	
	bool continueRotation(){
		// Raycast in front
		if (nextDirection != Vector3.zero){
			if (Vector3.Angle(transform.forward, nextDirection) > 1f){
				Quaternion targetRotation = Quaternion.LookRotation(nextDirection);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
				return true;
			} else{
				nextDirection = Vector3.zero;
				return false;
			}
		}
		return false;
	}

	bool walkStraight(){
		// Raycast in front
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(transform.position, transform.forward, out hit)){
			float hitDistanceForward = Vector3.Distance(hit.point, transform.position);
			if (hitDistanceForward < wallBuffer){
				return false;
			}
		}
		// didn't hit a wall. keep moving forward.
		guardController.Move(transform.forward.normalized * moveSpeed * Time.deltaTime);
		return true;
	}

	public void randomWalk(){
		List<Vector3> directionList = new List<Vector3>();
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(transform.position, new Vector3(0,0,1), out hit)){
			float hitDistanceNorth = Vector3.Distance(hit.point, transform.position);
			if (hitDistanceNorth > wallBuffer){
				directionList.Add(new Vector3(0,0,1));
			}
		}
		if (Physics.Raycast(transform.position, new Vector3(0,0,-1), out hit)){
			float hitDistanceSouth = Vector3.Distance(hit.point, transform.position);
			if (hitDistanceSouth > wallBuffer){
				directionList.Add(new Vector3(0,0,-1));
			}
		}
		if (Physics.Raycast(transform.position, new Vector3(-1,0,0), out hit)){
			float hitDistanceWest = Vector3.Distance(hit.point, transform.position);
			if (hitDistanceWest > wallBuffer){
				directionList.Add(new Vector3(-1,0,0));
			}
		}
		if (Physics.Raycast(transform.position, new Vector3(1,0,0), out hit)){
			float hitDistanceEast = Vector3.Distance(hit.point, transform.position);
			if (hitDistanceEast > wallBuffer){
				directionList.Add(new Vector3(1,0,0));
			}
		}
		if (directionList.Count > 0){
			int rndnum = Random.Range(0, directionList.Count);
			nextDirection = directionList[rndnum];
		}
	}

	// use these 2 functions to making it go a certain direction.
	public void walkTowards(Transform targetTransform){
		nextDirection = targetTransform.position - transform.position;
	}
	public void walkInDirection(Vector3 direction){
		nextDirection = direction;
	}
}

/* Hey, Zhan! as of right now, there's only a few thing left to implement the node system. You can use
 * Utility.selectNode (gameobject, *distance to detect nodes within (a float)*, stepsInRoom, lastNode)
 * to get a GameObject (a random eligible node within specified distance). If you increment stepsInRoom by one
 * each time you select/move to one, it'll even move you in and out of rooms appropriately.
 * I still haven't implemented a reset for that, because I'll have to do that in this code, and I don't wanna fuck around
 * with it too much. The pseudo-code implementation will be:
 * 
 * if((Utility.SelectNode).GetComponent<NodeScript>().isSuper && lastNode.GetComponent<NodeScript>().isSuper){
 * 		stepsInRoom = 0;
 * 	}

Because the node selection code depends on nodes being the last node and on/off, I can't test this yet. I managed
to test it up to the part where it accurately senses all eligible nodes, but I haven't tested selection. If you can get the guard
walking to/through nodes, I can finish this up. */