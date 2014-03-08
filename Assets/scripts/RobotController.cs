using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class RobotController: MonoBehaviour
{

	// public variables

    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public bool enableRunning = false;
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
    private float speed;
    private RaycastHit hit;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl = false;
    private int jumpTimer;
 
    void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
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
 
            if( enableRunning )
            {
            	speed = Input.GetButton("Run")? runSpeed : walkSpeed;
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
 
    // storing is grounded check
    void OnControllerColliderHit (ControllerColliderHit hit) {
        contactPoint = hit.point;
    }
}