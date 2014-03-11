using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class RobotController: MonoBehaviour
{

	// public variables

	// powerups
	public int powerUpCounter = 0;  // for endgame score

	public float speed = 4.0f;
	public float maxSpeed = 16.0f;
	public float lightBattery = 0.0f;
	public float maxBattery = 10.0f;
	public int stunGunAmmo = 0;
	public Light flashLight;

	// stungun stuff
	Texture2D crosshairTexture;
	Rect crosshairPosition;
	GUIText ammoText;
	Transform playerCameraTransform;

	// others

    public float jumpSpeed = 4.0f;
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
		// set up powerups
		flashLight = transform.GetComponentInChildren<Light>();
		flashLight.enabled = false;

		// load crosshair texture from resources and set position
		crosshairTexture = (Texture2D)Resources.Load("crosshair");
		crosshairPosition = new Rect((Screen.width - crosshairTexture.width)/2,(Screen.height - crosshairTexture.height)/2, 
		                             crosshairTexture.width, crosshairTexture.height);
		playerCameraTransform = transform.Find("Main Camera").transform;


		// ammo counter 
		GameObject ammoTextObj = new GameObject("ammoCounter");
		ammoTextObj.transform.position = new Vector3(0.5f,0.5f,0f);
		ammoText = (GUIText)ammoTextObj.AddComponent(typeof(GUIText));
		ammoText.pixelOffset = new Vector2(-440, -260);
		ammoText.fontSize = 18;
		ammoText.color = Color.green;
		ammoText.text = "Ammo: " + stunGunAmmo;

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
			if(flashLight.enabled == false){
				if(lightBattery > 0.0f){
					flashLight.enabled = true;
				}
			} else{
				flashLight.enabled = false;
			}
		}
		// stun gun fire
		if(Input.GetMouseButtonDown(0)){
			if(stunGunAmmo > 0){
				Ray mouseRay = new Ray(playerCameraTransform.position, playerCameraTransform.forward);
				RaycastHit rayHit = new RaycastHit();
				if(Physics.Raycast(mouseRay, out rayHit, 200f)){
					if (rayHit.transform.GetComponent<TrialPatrol>() != null){
						rayHit.transform.GetComponent<TrialPatrol>().stun(5.0f);
					}
				}
				stunGunAmmo --;
				ammoText.text = "Ammo: " + stunGunAmmo;
			}
		}
	}
 
    // storing is grounded check
    void OnControllerColliderHit (ControllerColliderHit hit) {
        contactPoint = hit.point;
    }
	
	void OnGUI(){
		// draw crosshair if there's ammo left
		if(stunGunAmmo > 0){
			GUI.DrawTexture(crosshairPosition, crosshairTexture);
		}

//		// show flashlight battery
//		GUI.Box(new Rect(10, 10, Screen.width/2, 20), lightBattery + "/" + maxBattery);
	}
	
	public void PowerUp(int PowerUpType, float PowerUpValue){
		if (PowerUpType == 0){
			// wheels
			if (speed + PowerUpValue > maxSpeed){
				speed = maxSpeed;
			} else{
				speed += PowerUpValue;
			}
		} else if (PowerUpType == 1){
			// flashlight
			if (lightBattery + PowerUpValue > maxBattery){
				lightBattery = maxBattery;
			} else{
				lightBattery += PowerUpValue;
			}
		} else if (PowerUpType == 2){
			// stun gun
			int ammo = (int)PowerUpValue;
			stunGunAmmo += ammo;
			ammoText.text = "Ammo: " + stunGunAmmo;
		}
		powerUpCounter ++;
	}
}