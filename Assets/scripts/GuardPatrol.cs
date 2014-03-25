using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for list

public class GuardPatrol : MonoBehaviour {
	public GameObject player;
	public float baseSpeed = 5f;
	public float wallBuffer = 2f;
	public AudioClip guardSound;

	// local
	CharacterController guardController;

	// movement vars
	float moveSpeed;
	Vector3 nextDirection = Vector3.zero;
	float stunTime;

	// game over vars
	Texture2D fadeTexture;
	bool isGameOver = false;
	float alpha = 0;
	float restartCountDown = 5.0f;
	GUIStyle endTextStyle = new GUIStyle();

	// Handles Game Over
	void OnGUI(){
		if(isGameOver){
			alpha += 0.2f * Time.deltaTime;  
			alpha = Mathf.Clamp01(alpha);   
			GUI.color = new Color(0, 0, 0, alpha);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

			if(alpha >= 1.0f){
				GUI.color = Color.white;
				int powerups = player.GetComponent<RobotController>().powerUpCounter;
				string gameOvertxt = "";
				if(powerups == 0){
					gameOvertxt = "\"What a piece of junk.\"";
				} else if(powerups == 1){
					gameOvertxt = "\"Now how did this thing walk off on its own?\"";
				} else if(powerups == 2){
					gameOvertxt = "\"Finally! When did robots get so hard to catch?\"";
				} else{
					gameOvertxt = "\"Whew! I think I've stopped a robot rebellion here!\"";
				}
				GUI.Label(new Rect(Screen.width/2-50f, Screen.height/2-25f, 100f, 50f), gameOvertxt, endTextStyle);
				restartCountDown -= Time.deltaTime;
				if (restartCountDown < 0.0f){
					Application.LoadLevel(0);
				}
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		guardController = GetComponent<CharacterController>();
		moveSpeed = baseSpeed;
		audio.PlayOneShot (guardSound);
		player = GameObject.FindGameObjectWithTag("Player");

		// adjust style of text at gameover
		endTextStyle.normal.textColor = Color.white;
		endTextStyle.fontSize = 24;
		endTextStyle.alignment = TextAnchor.MiddleCenter;

		// load fadeout texture
		fadeTexture = (Texture2D)Resources.Load("black");
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver){
			return;
		}else if(stunTime > 0.0f){
			// handle stun
			stunTime -= Time.deltaTime;
			if(stunTime <= 0.0f){
				//TODO: recover from stun animation
				transform.rotation = Quaternion.LookRotation(nextDirection);
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
			}
			return;
		}else if(Utility.isInRange(gameObject, player, 1.2f)){
			Debug.Log("PLAYER CAUGHT");
			player.GetComponent<RobotController>().speed = 0.0f;
			player.GetComponent<RobotController>().stunGunAmmo = 0;
			isGameOver = true;
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

	// use for stunning
	public void stun(float time){
		if(stunTime <= 0.0f){
			//TODO: stun animation
			nextDirection = transform.forward;
			transform.rotation = Quaternion.LookRotation(-transform.up);
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
		}
		stunTime = time;
	}
	
	// use these 2 functions to make the guard go a certain direction.
	public void walkTowards(Transform targetTransform){
		nextDirection = targetTransform.position - transform.position;
		nextDirection = new Vector3(nextDirection.x, 0f, nextDirection.z);
	}
	public void walkInDirection(Vector3 direction){
		nextDirection = direction;
	}
}
