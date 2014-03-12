using UnityEngine;
using System.Collections;

public class Escape : MonoBehaviour {
	Transform player;

	Texture2D fadeTexture;
	bool win = false;
	float alpha = 0;
	float textCountDown = 10.0f;
	float restartCountDown = 16.0f;
	GUIStyle endTextStyle = new GUIStyle();
	// Use this for initialization
	void Start () {
		// adjust style of text at win
		endTextStyle.normal.textColor = Color.black;
		endTextStyle.fontSize = 24;
		endTextStyle.alignment = TextAnchor.MiddleCenter;
		endTextStyle.fontStyle = FontStyle.Bold;
		
		// load fadeout texture
		fadeTexture = (Texture2D)Resources.Load("white");
	}

	// Handles Game Over
	void OnGUI(){
		if(win && transform.position.y > 3f){
			alpha += 0.2f * Time.deltaTime;  
			alpha = Mathf.Clamp01(alpha);   
			GUI.color = new Color(255, 255, 255, alpha);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
			
			if(alpha >= 1.0f){
				GUI.color = Color.white;
				int powerups = player.GetComponent<RobotController>().powerUpCounter;
				string winTxt = "March 12, 2172.\n The first sentient robot escaped from a factory.";
				textCountDown -= Time.deltaTime;
				Color color;
				// fade out first text
				if (textCountDown < 2.5f && textCountDown >= 0f){
					color = Color.black;
					color.a = 0.4f * textCountDown;
					color.a = Mathf.Clamp01(color.a);
					endTextStyle.normal.textColor = color;
				}
				if (textCountDown < 0f){
					restartCountDown -= Time.deltaTime;
					winTxt = "This event marked the beginning of mankind's extinction...";
					if (restartCountDown > 11f){
						// fade in second text
						color = Color.black;
						color.a = -0.2f * textCountDown;
						color.a = Mathf.Clamp01(color.a);
						endTextStyle.normal.textColor = color;
					} else{
						// fade out second text
						color = Color.black;
						color.a = 0.2f * (restartCountDown-6f);
						color.a = Mathf.Clamp01(color.a);
						endTextStyle.normal.textColor = color;
					}
					if (restartCountDown < 0.0f){
						Application.LoadLevel(0);
					}
				}
				GUI.Label(new Rect(Screen.width/2-75f, Screen.height/2-25f, 150f, 50f), winTxt, endTextStyle);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(win){
			//raise platform slowly
			transform.Translate(new Vector3(0f,1.5f * Time.deltaTime,0f));
			player.Translate(new Vector3(0f,1.5f * Time.deltaTime,0f));
		}
	}

	void OnTriggerEnter (Collider other){
		if (other.transform.tag == "Player"){
			player = other.transform;
			RobotController playerControl = player.GetComponent<RobotController>();
			playerControl.speed = 0.5f;
			win = true;
		}
	}
}
