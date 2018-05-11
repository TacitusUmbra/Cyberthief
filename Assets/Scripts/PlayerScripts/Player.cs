using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour {
//Making a place to set the PlayerInput so that the player can get access to the inputs
public PlayerInput pi;
public PlayerConfig pc;
public ShadowBufferScript sbs;
private CharacterController cc;
private Transform anchorTransform;
public float maxPitch;
public float minPitch;
private Camera cam;
public LayerMask camLayerMask;
private float originalCameraDistance;
public State currentState;
public State defaultState = State.Idle;
public float floorRayDistance;
public float visibility;
public float visibilityModifier;

[Header("Player Crouch")]
public bool crouchBump;
public bool isCrouched = false;
public float crouchHeight;
public float standHeight;
public float downLerpTime;
public float standLerpTime;
public float crouchRayDistance;
public float standVisibilityModifier;
public float crouchVisibiltyModifier;
public LayerMask crouchLayer;

//Here is a header with all the plyer movements, walkspeed, runspeed,  the gravity, and the jumpstrength
[Header("Player Movement")]
public bool Grounded;
public float currentSpeed;
public float walkspeed;
public float runSpeed;
public float crouchSpeed;
public float walkLerpTime;
public float runLerpTime;
public float crouchLerpTime;

[Header("Player Jump")]
public float currentGravity;
public float jumpStrength;
public float gravityForce;

public Vector3 currentDirection;
//State machine with the Idle, Walk, Jump, and Run
public enum State {
Idle,
Walk,
Jump,
Run,
Crouch
}


void Awake () {
Cursor.visible = false;
Cursor.lockState = CursorLockMode.Locked;
crouchBump = false;
isCrouched = false;
//The currentState is the defaultState on Awake, which is Idle
this.currentState = this.defaultState;
//cc is short for the CharacterController 
this.cc = this.GetComponent<CharacterController>();
//cam is short for the Camera we will get inside of the child
this.cam= this.GetComponentInChildren<Camera>();
//anchorTransform is short for the camera transform in the parent
this.anchorTransform = this.cam.transform.parent;
//the originalCameraDistance is goign to set the distance of the camera that is goes back to if nothing is in the way
this.originalCameraDistance = (this. cam.transform.parent.position -  this.cam.transform.position).magnitude;
}
void FixedUpdate ()
{
visibility = sbs.percentageHidden * visibilityModifier;
//If the character is grounded, turn the bool grounded to true, otherwise, it is false because they are not grounded.
if (cc.isGrounded == true) 
Grounded = true;
else
Grounded = false;

//The states that switch between one another, update on each frame.
switch (this.currentState) {
case State.Idle: this.Idle(); break;
case State.Walk: this.Walk(); break;
case State.Jump: this.Jump(); break;
case State.Run: this.Run(); break;
case State.Crouch: this.Crouch(); break;
}

//Altering the height of the character controller when the character is crouched or not
if (isCrouched)
{
cc.height = Mathf.Lerp (cc.height, crouchHeight, downLerpTime * Time.deltaTime);
visibilityModifier = crouchVisibiltyModifier;
}

else{
cc.height = Mathf.Lerp (cc.height, standHeight, standLerpTime * Time.deltaTime);
visibilityModifier = standVisibilityModifier;
}

//If the character isn't grounded, they will fall
if (!cc.isGrounded)
currentGravity += gravityForce * Time.deltaTime;
//Calling on the AdjustCamera each frame.
this.AdjustCamera();
//Updating the direction each frame.
currentDirection = (transform.rotation * this.pi.direction);
currentDirection = new Vector3(currentDirection.x*currentSpeed, currentDirection.y-currentGravity, currentDirection.z*currentSpeed);
currentDirection *= Time.deltaTime;

//Checking the motion each frame
this.cc.Move (currentDirection);

//Checking the rotation each frame
this.transform.rotation *= Quaternion.Euler (0f, this.pi.rotation.x, 0f);
//Checking the camera's rotation each frame
this.cam.transform.parent.rotation *= Quaternion.Euler (this.pi.rotation.y, 0f, 0f);

// ( <-> max pitch && <180) || (min pitch <-> 360 && > 180)
Vector3 angles = this.anchorTransform.localRotation.eulerAngles;
if (angles.x >= 0 && angles.x < 180f && angles.x > this.maxPitch) {
this.anchorTransform.localRotation = Quaternion.Euler (this.maxPitch, 0f, 0f);
} else if (angles.x < this.minPitch && angles.x > 180f && angles.x <= 360) {
this.anchorTransform.localRotation = Quaternion.Euler (this.minPitch, 0f, 0f);
} 
}




//This is the Idle. It will have these conditions when inside the Idle state.
void Idle ()
{
currentSpeed = walkspeed;
//If the character is moving in the x or z direction, the currenstate will become Walk
if (this.pi.direction.x != 0 || this.pi.direction.z != 0) {
this.currentState = State.Walk;
//If the character is grounded and they press the key that corresponds with jump in the PlayerConfig, they will jump and teh currenState will become jump.
} else if (Input.GetKeyDown (this.pc.jump) && cc.isGrounded) {
currentGravity = jumpStrength;
this.currentState = State.Jump;
}else if (Input.GetKeyDown (this.pc.crouch) && cc.isGrounded){
this.currentState = State.Crouch;
}else if (Input.GetKeyDown(this.pc.run) && cc.isGrounded){
this.currentState = State.Run;
}
}



//This is the Walk. It will have these conditions when inside the Walk state.
void Walk () 
{
currentSpeed = Mathf.Lerp (currentSpeed, walkspeed, walkLerpTime * Time.deltaTime);

RaycastHit floorHit;
Ray floorRay = new Ray(transform.position, Vector3.down);
if (Physics.Raycast (floorRay, out floorHit, floorRayDistance))
{
if (floorHit.collider.gameObject.name == "Concrete") {
}
if (floorHit.collider.gameObject.name == "Water") {
}
}

//If the player ceases to move in the x or z direction, the currentState will become Idle.
if (this.pi.direction.x == 0 && this.pi.direction.z == 0)
{
this.currentState = State.Idle;
} else if (Input.GetKeyDown (this.pc.jump) && cc.isGrounded) 
{
currentGravity = jumpStrength;
this.currentState = State.Jump;
} else if (Input.GetKeyDown(this.pc.run)&& crouchBump == false) 
{
this.currentState = State.Run;
}else if (Input.GetKeyDown (this.pc.crouch) && cc.isGrounded)
{
this.currentState = State.Crouch;
}

}



//This is the Jump. It will have these conditions in the Jump State.
void Jump () 
{
//If the character is moving in the x or z direction, the currenstate will become Walk.
if (this.cc.isGrounded == false && this.pi.direction.magnitude > 0.1f) {
} else 
{
this.pi.direction = Vector3.zero;
this.currentState = State.Walk;
}
}




//This is the Run. It will have these conditions in the Run state.
void Run () 
{
//If the character is moving in the x or z direction, the currenstate will become Walk.
if (Input.GetKey (KeyCode.LeftShift))
{
currentSpeed = Mathf.Lerp (currentSpeed, runSpeed, runLerpTime * Time.deltaTime);
} else
{ 
this.currentState = State.Walk;
}
if (Input.GetKeyDown (this.pc.jump) && cc.isGrounded) 
{
currentGravity = jumpStrength;
this.currentState = State.Jump;
}
}



void Crouch(){
Vector3 up = transform.TransformDirection (Vector3.up);
RaycastHit crouchHit;
Ray crouchRay = new Ray(transform.position, up);
if (Physics.Raycast (crouchRay, out crouchHit, crouchRayDistance, crouchLayer)) 
{
if(crouchHit.collider) 
{
crouchBump = true;
} 
}else 
{
crouchBump = false;
} 
isCrouched = true;

currentSpeed = Mathf.Lerp (currentSpeed, crouchSpeed, crouchLerpTime * Time.deltaTime);

if (Input.GetKeyDown (this.pc.crouch) && cc.isGrounded && isCrouched && !crouchBump)
{
isCrouched = false;
this.currentState = State.Idle;
}
else if(Input.GetKey(this.pc.run)&& !crouchBump)
{
isCrouched = false;
this.currentState = State.Run;
}
else if (Input.GetKeyDown (this.pc.jump) && cc.isGrounded && !crouchBump) 
{
cc.height = Mathf.Lerp (cc.height, standHeight, standLerpTime * Time.deltaTime);
isCrouched = false;
currentGravity = jumpStrength;
this.currentState = State.Jump;
}
}



//Adjusts the camera if it collides with objects
void AdjustCamera()
{

//Calls the RaycastHit hit to use it easier.
RaycastHit hit;
//direction is the camera position minus the camera's parent position
Vector3 direction = this.cam.transform.position - this.cam.transform.parent.position;
//Setting up the origin
Vector3 origin = this.cam.transform.parent.position;
// If the camera hits something, it will move closer to the origin, otherwise, it will move away.
        if (Physics.SphereCast(origin, 0.35f, direction, out hit, this.originalCameraDistance, this.camLayerMask.value)) 
{
Vector3 newVector = direction.normalized  * hit.distance;
this.cam.transform.position = newVector + this.cam.transform.parent.position;
}else{
Vector3 newVector = direction.normalized * this.originalCameraDistance;
this.cam.transform.position = newVector + this.cam.transform.parent.position;
}
}

}