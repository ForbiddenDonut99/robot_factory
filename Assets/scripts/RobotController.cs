using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class RobotController: MonoBehaviour
{

	// public variables

	// powerups
	public int powerUpCounter = 0;  // for endgame score
	public float speed = 7.0f;
	public float maxSpeed = 14.0f;
	public bool scopeEnabled = false;
	public bool compassEnabled = false;
	public int stunGunAmmo = 0;
	public int normalCameraZoom = 75;
	public int largeCameraZoom = 30;
	public float zoomSpeed = 10f;
	Light flashLight;
	GUIText powerupText;
	float powerupFadeAlpha = 0f;

	// to swap powerups
	BuildingGen buildingGenerator;

	// compass stuff
	Texture2D compassTexture;
	Transform goalTransform;
	
	// stungun stuff
	Texture2D crosshairTexture;
	Rect crosshairPosition;
	GUIText ammoText;
	Camera playerCamera;

	// gameover
	public bool isWin = false;
	public bool isGameOver = false;
	float alpha = 0;
	float restartCountDown = 5.0f;
	GUIStyle endTextStyle = new GUIStyle();
	Texture2D fadeTexture;

	// others

	public float jumpSpeed = 6.0f;
	public float maxJumpSpeed = 12.0f;
	public float gravity = 10.0f; 
    public bool slideWhenOverSlopeLimit = false;
	public bool slideOnTaggedObjects = false;
    public float slideSpeed = 5.0f;
	public bool airControl = true;
    public float antiBumpFactor = .75f;
    public int antiBunnyHopFactor = 1;

	// private variables

	private bool limitDiagonalSpeed = true;
    private Vector3 moveDirection = Vector3.zero;
    private bool grounded = false;
    private CharacterController controller;
    private Transform myTransform;
    private RaycastHit hit;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl = false;
    private int jumpTimer;
 
    void Start()
	{
		buildingGenerator = GameObject.Find("Generator").GetComponent<BuildingGen>();

		// set up powerups
		flashLight = transform.GetComponentInChildren<Light>();
		flashLight.enabled = false;

		// compass variables
		compassTexture = (Texture2D)Resources.Load("arrow");
		goalTransform = GameObject.FindGameObjectWithTag("Finish").transform;
		
		// load crosshair texture from resources and set position
		crosshairTexture = (Texture2D)Resources.Load("crosshair");
		crosshairPosition = new Rect((Screen.width - crosshairTexture.width)/2,(Screen.height - crosshairTexture.height)/2, 
		                             crosshairTexture.width, crosshairTexture.height);
		playerCamera = transform.Find("Main Camera").GetComponent<Camera>();


		// ammo counter 
		GameObject ammoTextObj = new GameObject("ammoCounter");
		ammoTextObj.transform.position = new Vector3(0.5f,0.5f,0f);
		ammoText = (GUIText)ammoTextObj.AddComponent(typeof(GUIText));
		ammoText.pixelOffset = new Vector2(-Screen.width/2 + 40, -Screen.height/2 + 40);
		ammoText.fontSize = 18;
		ammoText.color = Color.white;
		ammoText.text = "";

		// powerup alert style
		GameObject powerupObj = new GameObject("powerupText");
		powerupObj.transform.position = new Vector3(0.5f,0.5f,0f);
		powerupText = (GUIText)powerupObj.AddComponent(typeof(GUIText));
		powerupText.pixelOffset = new Vector2(Screen.width/2 - 40, Screen.height/2 - 40);
		powerupText.fontSize = 24;
		powerupText.fontStyle = FontStyle.Bold;
		powerupText.anchor = TextAnchor.LowerRight;
		Color color = Color.green;
		color.a = powerupFadeAlpha;
		powerupText.color = color;
		powerupText.text = "";

		// gameover text style
		endTextStyle.normal.textColor = Color.white;
		endTextStyle.fontSize = 24;
		endTextStyle.alignment = TextAnchor.MiddleCenter;
		fadeTexture = Resources.Load<Texture2D>("black");

		// the rest
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        rayDistance = controller.height * .5f + controller.radius;
        slideLimit = controller.slopeLimit - .1f;
        jumpTimer = antiBunnyHopFactor;
    }
 
    void FixedUpdate() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed)? .7071f : 1.0f;
 
        if (grounded) {
            bool sliding = false;
      
            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance)) {
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }
           
            else {
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
			}
 
            
            if ( (sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide") ) {
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
           
            else {
                moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
                moveDirection = myTransform.TransformDirection(moveDirection) * speed;
                playerControl = true;
            }
 
            
            if (!Input.GetButton("Jump"))
                jumpTimer++;
            else if (jumpTimer >= antiBunnyHopFactor) {
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
            }
        }
        else {
            
            if (!falling) {
                falling = true;
            }
           
            if (airControl && playerControl) {
                moveDirection.x = inputX * speed * inputModifyFactor;
                moveDirection.z = inputY * speed * inputModifyFactor;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }
 
        // gravity
        moveDirection.y -= gravity * Time.deltaTime;
 
        // is grounded check
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

	void Update(){
		// flashlight switch
		if (Input.GetKeyDown(KeyCode.F)){
			flashLight.enabled = !flashLight.enabled;
		}

		// stun gun fire
		if(Input.GetMouseButtonDown(0)){
			if(stunGunAmmo > 0){
				Ray mouseRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
				RaycastHit rayHit = new RaycastHit();
				if(Physics.Raycast(mouseRay, out rayHit, 200f)){
					if (rayHit.transform.GetComponent<TrialPatrol>() != null){
						rayHit.transform.GetComponent<TrialPatrol>().stun(5.0f);
					} else if (rayHit.transform.GetComponent<GuardPatrol>() != null){
						rayHit.transform.GetComponent<GuardPatrol>().stun(5.0f);
					} 
				}
				stunGunAmmo --;
				ammoText.text = "Ammo: " + stunGunAmmo;
			}
		}

		// zoom
		if(scopeEnabled){
			if(Input.GetMouseButton(1)){
				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView,largeCameraZoom,Time.deltaTime*zoomSpeed);
			} else{
				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView,normalCameraZoom,Time.deltaTime*zoomSpeed);
			}
		}

		//powerup pickup alert fade
		if(powerupFadeAlpha > 0f){
			powerupFadeAlpha -= Time.deltaTime * 0.5f;
			Color color = Color.green;
			color.a = Mathf.Clamp01(powerupFadeAlpha);
			powerupText.color = color;
		}
	}
 
    // storing is grounded check
    void OnControllerColliderHit (ControllerColliderHit hit) {
        contactPoint = hit.point;
    }
	
	void OnGUI(){
		if(isGameOver){
			alpha += 0.2f * Time.deltaTime;  
			alpha = Mathf.Clamp01(alpha);   
			GUI.color = new Color(0, 0, 0, alpha);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
			
			if(alpha >= 1.0f){
				GUI.color = Color.white;
				string gameOvertxt = "";
				if(powerUpCounter == 0){
					gameOvertxt = "\"What a piece of junk.\"";
				} else if(powerUpCounter == 1){
					gameOvertxt = "\"Now how did this thing walk off on its own?\"";
				} else if(powerUpCounter == 2){
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

		if (compassEnabled){
			Vector2 goalDirection = new Vector3(goalTransform.position.x - transform.position.x,goalTransform.position.z - transform.position.z);
			Vector2 forwardDirection = new Vector3(transform.forward.x,transform.forward.z);
			float angle = Vector2.Angle(goalDirection,forwardDirection);
			Vector3 cross = Vector3.Cross(goalDirection,forwardDirection);
//			Debug.Log("angle: " + angle + "cross: " + cross);
			if(cross.z < 0f){
				angle = -angle;
			}
			Matrix4x4 matrixBackup = GUI.matrix;
			GUIUtility.RotateAroundPivot(angle, new Vector2(30,30));
			GUI.DrawTexture(new Rect(10, 10, 40, 40), compassTexture);
			GUI.matrix = matrixBackup;
		}
		
		// draw crosshair if there's ammo left
		if(stunGunAmmo > 0){
			GUI.DrawTexture(crosshairPosition, crosshairTexture);
		}
	}

	public void GameOver(){
		if(!isWin){
			speed = 0.0f;
			stunGunAmmo = 0;
			compassEnabled = false;
			isGameOver = true;
		}
	}
	
	public void GetPowerUp(GameObject thePowerup){
		int PowerUpType = thePowerup.GetComponent<PowerUp>().PowerUpType;
		float PowerUpValue = thePowerup.GetComponent<PowerUp>().PowerUpValue;
		buildingGenerator.removePowerup(thePowerup);
		Destroy(thePowerup);
		if (PowerUpType == PowerUp.POWERUPTYPEWHEEL){
			// wheels
			powerupText.text = "Wheels! Speed Up!";
			powerupFadeAlpha = 2f;
			if (speed + PowerUpValue >= maxSpeed){
				speed = maxSpeed;
				buildingGenerator.GetComponent<BuildingGen>().PowerUpSwap(PowerUpType);
			} else{
				speed += PowerUpValue;
			}
		} else if (PowerUpType == PowerUp.POWERUPTYPESTUNGUN){
			// stun gun
			powerupText.text = "Stun Gun! Ammo Up!";
			powerupFadeAlpha = 2f;
			int ammo = (int)PowerUpValue;
			stunGunAmmo += ammo;
			ammoText.text = "Ammo: " + stunGunAmmo;
		} else if (PowerUpType == PowerUp.POWERUPTYPESCOPE){
			// flashlight
			powerupText.text = "Scope! Zoom in with right click.";
			powerupFadeAlpha = 2f;
			scopeEnabled = true;
			buildingGenerator.PowerUpSwap(PowerUpType);
		} else if (PowerUpType == PowerUp.POWERUPTYPECOMPASS){
			// compass
			powerupText.text = "Compass! Target: Exit.";
			powerupFadeAlpha = 2f;
			compassEnabled = true;
			buildingGenerator.PowerUpSwap(PowerUpType);
		} else if (PowerUpType == PowerUp.POWERUPTYPESPRING){
			// super jump
			powerupText.text = "Spring! Jump Higher!";
			powerupFadeAlpha = 2f;
			if (jumpSpeed + 3f >= maxJumpSpeed){
				jumpSpeed = maxJumpSpeed;
				buildingGenerator.PowerUpSwap(PowerUpType);
			} else{
				jumpSpeed += 3f;
			}
		}
		powerUpCounter ++;
	}
}