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
	public float rotation = 2.0f;
	Texture2D fadeTexture;
	float moveSpeed;
	CharacterController guardController;
	// Use this for initialization
	void Start () {
		fadeTexture = Resources.Load<Texture2D>("black");
		state = "MoveToNode";
		moveSpeed = baseSpeed;
		guardController = GetComponent<CharacterController>();
		player = GameObject.FindGameObjectWithTag("Player");

		endTextStyle.normal.textColor = Color.white;
		endTextStyle.fontSize = 24;
		endTextStyle.alignment = TextAnchor.MiddleCenter;

		//Find an initial node to be lastNode
		targetNode = Sense.startNode(gameObject, Sense.nearbyNodes(gameObject, nodeDistance));
	}
	
	// Update is called once per frame
	void Update () {

		//Check for game over.
		if(Vector3.Distance(transform.position, player.transform.position) < 1.2f && !(stunTime > 0.0f)){
			Debug.Log("PLAYER CAUGHT");
			player.GetComponent<RobotController>().speed = 0.0f;
			player.GetComponent<RobotController>().stunGunAmmo = 0;
			isGameOver = true;
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
			Debug.Log ("Hunting Player!");
			rotation = 8.0f;
		}
		else rotation = 2.0f;

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
			if (!Sense.player(gameObject, player)){
				state = "FindNode";
				Debug.Log ("Lost sight of Player.");
			}
			else{
				guardController.Move (Vector3.Normalize(player.transform.position - transform.position)*Time.deltaTime*moveSpeed*1.5f);
				Vector3 finalFacing = (player.GetComponent<Transform>().position - transform.position).normalized;
				transform.forward = Vector3.Lerp (transform.forward, finalFacing, Time.deltaTime*rotation);
			}
			break;
		
		
		}
	}

	bool isGameOver = false;
	float alpha = 0;
	float restartCountDown = 5.0f;
	GUIStyle endTextStyle = new GUIStyle();

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
