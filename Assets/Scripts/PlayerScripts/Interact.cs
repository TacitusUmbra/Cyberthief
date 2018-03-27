using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {



	public PlayerConfig pc;
	public float interactionDistance;
	public Player pl;
	public LayerMask camLayerMask;
	public Inventory inventory;
	

	[Header("People Choking")]
	public float chokeTimer;
	public float chokeTime;
	public GameObject chokeTarget;


	[Header("Holding Objects")]
	public bool canGrab;
	public GameObject objectHeld;
	public float grabSpeed;
	public Transform grabLocation;
	public float thrust;

	[Header("Holding Bodies")]
	public GameObject bodyHeld;
	public Transform grabBodyLocation;
	public float grabBodySpeed;
	public float bodyThrust;



	void Start () 
	{
		canGrab = true;
	}
	
	void Update ()
	{
		//If you're holding a body, you can throw it by clicking the Use button
		if (bodyHeld)
		{

			bodyHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Held;
			bodyHeld.transform.position = Vector3.Lerp (bodyHeld.transform.position, grabBodyLocation.position, grabBodySpeed * Time.deltaTime);
			bodyHeld.transform.rotation = grabBodyLocation.transform.rotation;

			if(Input.GetKeyDown(pc.use))
			{
				bodyHeld.GetComponent<Rigidbody> ().isKinematic = false;
				bodyHeld.GetComponent<Rigidbody> ().AddForce (transform.forward * bodyThrust,ForceMode.Impulse);
				canGrab = true;
				bodyHeld.GetComponent<Rigidbody>().useGravity = true;
				bodyHeld.transform.parent = null;
				bodyHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Dropped;
				bodyHeld = null;
			}
		}
		//The interact ray
		Vector3 forward = transform.TransformDirection (Vector3.forward);
		RaycastHit interactHit;
		Ray interactRay = new Ray (transform.position, forward);
		if (Physics.Raycast (interactRay, out interactHit, interactionDistance,camLayerMask))
		{
			        Debug.DrawRay(transform.position, forward, Color.green);

			if (interactHit.collider.tag == "Lightswitch")
			{
				if (Input.GetKeyDown (pc.interact))
				{
					if (interactHit.collider.gameObject.GetComponent<LightSwitch> ().switchState)
					{
						interactHit.collider.gameObject.GetComponent<LightSwitch> ().switchState = false;
					}
					else
						{
							interactHit.collider.gameObject.GetComponent<LightSwitch> ().switchState = true;
						}
				}
			}

			//If conditions are met, pickup an object
			if (interactHit.collider.tag == "Grabbable" && (Input.GetKeyDown (pc.interact)) && canGrab)
			{
				objectHeld = interactHit.collider.gameObject;
				objectHeld.transform.SetParent(grabLocation);
				canGrab = false;
				objectHeld.GetComponent<Rigidbody>().useGravity = false;
			}
			//If you're holding something and there is an object being held, have it fall into your hands.
			if(objectHeld)
			{
				objectHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Held;
				objectHeld.transform.position = Vector3.Lerp (objectHeld.transform.position, grabLocation.position, grabSpeed * Time.deltaTime);

				if(objectHeld.gameObject.GetComponent<GrabbableObject>().hit == true)
					{
						canGrab = true;
						objectHeld.GetComponent<Rigidbody>().useGravity = true;
						objectHeld.transform.parent = null;
						objectHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Dropped;
						objectHeld = null;
					}
			//Throwing a gameobject that is being held		
				if(Input.GetKeyDown(pc.use))
					{
						objectHeld.GetComponent<Rigidbody> ().AddForce (transform.forward * thrust,ForceMode.Impulse);
						canGrab = true;
						objectHeld.GetComponent<Rigidbody>().useGravity = true;
						objectHeld.transform.parent = null;
						objectHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Thrown;
						objectHeld = null;
					}
			//It the player presses alternativeInteract, the object will unchild itself, being dropped
				if(Input.GetKeyDown (pc.alternativeInteract))
					{
						canGrab = true;
						objectHeld.GetComponent<Rigidbody>().useGravity = true;
						objectHeld.transform.parent = null;
						objectHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Dropped;
						objectHeld = null;
					}
			}
			
			//Opening Doors
			if (interactHit.collider.tag == "Door" && (Input.GetKeyDown (pc.interact)))
			{
				if(interactHit.collider.gameObject.GetComponent<Door> ().locked == false)
				{
				interactHit.collider.gameObject.GetComponent<Door> ().doorState = Door.State.Opening;
				}	
			}

			//Acessing Terminals to hack them only if you are holding the device
			if (inventory.equipState == Inventory.State.HoldDevice)
			{
				if (interactHit.collider.tag == "Terminal" && Input.GetKey (pc.interact))
				{
					if (interactHit.collider.gameObject.GetComponent<Terminal> ().hacked == false)
					{
						interactHit.collider.gameObject.GetComponent<Terminal> ().percentageHacked += 15f * Time.deltaTime;
					}
				}
			}
			//If all conditions are met, autohack the terminal, otherwise, display message saying there are not enough decoders
			if(interactHit.collider.tag == "Terminal" && Input.GetKeyDown(pc.alternativeInteract) && interactHit.collider.gameObject.GetComponent<Terminal>().autohack == false && inventory.numberOfDecoders > 0)
				{
					interactHit.collider.gameObject.GetComponent<Terminal>().autohack = true;
					inventory.numberOfDecoders = inventory.numberOfDecoders-1;
					Debug.Log("Autohacking!");
				}
			else if(interactHit.collider.tag == "Terminal" && Input.GetKey(pc.alternativeInteract) && inventory.numberOfDecoders == 0)
				{
					Debug.Log("Not Enough Decoders!");
				}

			//Picking up a keycard of a particular level and destroying it afterwards
			if(interactHit.collider.tag == "Keycard" && Input.GetKey(pc.interact))
			{
				if(interactHit.collider.gameObject.GetComponent<Keycard>().keycardLevel == 1 && inventory.keycardLevelOne == false)
				{
					inventory.keycardLevelOne = true;
					Destroy(interactHit.collider.gameObject);
				}
				if(interactHit.collider.gameObject.GetComponent<Keycard>().keycardLevel == 2 && inventory.keycardLevelOne == false)
				{
					inventory.keycardLevelTwo = true;
					Destroy(interactHit.collider.gameObject);
				}
			}	

			//Using Keycard on Keycard Panel
			if(interactHit.collider.tag == "Keycard Panel" && Input.GetKey(pc.interact))
			{
				if(interactHit.collider.gameObject.GetComponent<KeycardPanel>().keycardLevelRequired == 1 && inventory.keycardLevelOne == true)
					interactHit.collider.gameObject.GetComponent<KeycardPanel>().accessGranted = true;
				else if(interactHit.collider.gameObject.GetComponent<KeycardPanel>().keycardLevelRequired == 2 && inventory.keycardLevelTwo == true)
					interactHit.collider.gameObject.GetComponent<KeycardPanel>().accessGranted = true;	
			}



			//if you find an unconscious body, you can pick it up
			if (interactHit.collider.tag == "Unconscious Body" && (Input.GetKey (pc.interact)) && canGrab)
			{
				bodyHeld = interactHit.collider.gameObject;
				bodyHeld.transform.SetParent(grabBodyLocation);
				canGrab = false;
				bodyHeld.GetComponent<Rigidbody>().useGravity = false;
				bodyHeld.GetComponent<Rigidbody> ().isKinematic = true;
			}

			//Storing the hitzone as a target
			if (interactHit.collider.tag == "HitZone") 
				chokeTarget = interactHit.collider.gameObject;
			else
				chokeTarget = null;

			//if you're choking someone and you stop, they will recover and become hostile
			if(chokeTarget){

				if((Input.GetKey(pc.alternativeInteract)) && interactHit.collider.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState != PatrolAI.State.Unconscious)
				{	
					pl.isCrouched = false;
					chokeTarget.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Choking;
					chokeTime += 1 * Time.deltaTime;
					Debug.Log("Choking guard");
					if(chokeTime > chokeTimer)
					{
					chokeTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Unconscious;
					chokeTime = 0;
					Debug.Log("He's Unconscious");
					}
				}
				else if((!Input.GetKey(pc.alternativeInteract)) && chokeTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState == PatrolAI.State.Choking)

					{	
						chokeTime = 0;
						chokeTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Recover;
					}

			}
		
		}
	}

}
