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
	public float nearDistance;


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

	[Header("Interactions")]
	public GameObject doorText;
	public GameObject keycardText;
	public GameObject grabText;
	public GameObject objecHeldText;
	public GameObject chokeText;
	public GameObject unconsciousBody;
	public GameObject terminalText;
	public GameObject useKeycardText;
	public GameObject autohackText;

	[Header("Comlink Hacking")]
	public bool terminalHackMode;
	public bool comHackMode;
	public GameObject comlinkText;
	public string stringHack;
	public GameObject comlinkPercentage;
	public GameObject guardHacked;
	public float numberOfOrders;
	public bool canHackGuard;
	public float deviceDistance;
	public float comHackTimer;
	
	



	void Start () 
	{
		canGrab = true;
		doorText.SetActive(false);
		keycardText.SetActive(false);
		grabText.SetActive(false);
		objecHeldText.SetActive(false);
		comHackMode = false;
		terminalHackMode = true;
		comlinkText.SetActive(false);
		canHackGuard = true;

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
			if (interactHit.collider.tag == "Grabbable" && canGrab && interactHit.collider.gameObject.GetComponent<GrabbableObject>().objectState == GrabbableObject.State.Grounded)
			{
				grabText.SetActive(true);

				if(Input.GetKeyDown (pc.interact))
				{
				objectHeld = interactHit.collider.gameObject;
				objectHeld.transform.SetParent(grabLocation);
				canGrab = false;
				objectHeld.GetComponent<Rigidbody>().useGravity = false;
				}
			}
			else
			{
				grabText.SetActive(false);				
			}


			//If you're holding something and there is an object being held, have it fall into your hands.
			if(objectHeld)
			{
				objecHeldText.SetActive(true);
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
			else
			{
				objecHeldText.SetActive(false);
			}
			

			//Opening Doors
			if (interactHit.collider.tag == "Door")
			{
				doorText.SetActive(true);
				if(Input.GetKeyDown (pc.interact))
				{
					if(interactHit.collider.gameObject.GetComponent<Door> ().locked == false)
						{
						interactHit.collider.gameObject.GetComponent<Door> ().doorState = Door.State.Opening;
						}
				}	

			}
			else
			{
			doorText.SetActive(false);
			}

			//If all conditions are met, autohack the terminal, otherwise, display message saying there are not enough decoders
			if(interactHit.collider.tag == "Terminal" && Input.GetKeyDown(pc.alternativeInteract) && interactHit.collider.gameObject.GetComponent<Terminal>().autohack == false && inventory.numberOfDecoders > 0)
				{
					interactHit.collider.gameObject.GetComponent<Terminal>().autohack = true;
					inventory.numberOfDecoders = inventory.numberOfDecoders-1;
					
				}
			else if(interactHit.collider.tag == "Terminal" && Input.GetKey(pc.alternativeInteract) && inventory.numberOfDecoders == 0)
				{
				
				}

			//Picking up a keycard of a particular level and destroying it afterwards
			if(interactHit.collider.tag == "Keycard")
			{
				keycardText.SetActive(true);
				if(Input.GetKey(pc.interact))
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
			}
			else
			{
				keycardText.SetActive(false);
			}

			//Using Keycard on Keycard Panel
			if(interactHit.collider.tag == "Terminal")
			{
				useKeycardText.SetActive(true);

				if(Input.GetKey(pc.interact))
				{
					if(interactHit.collider.gameObject.GetComponent<KeycardPanel>().keycardLevelRequired == 1 && inventory.keycardLevelOne == true)
						interactHit.collider.gameObject.GetComponent<KeycardPanel>().accessGranted = true;
					else if(interactHit.collider.gameObject.GetComponent<KeycardPanel>().keycardLevelRequired == 2 && inventory.keycardLevelTwo == true)
						interactHit.collider.gameObject.GetComponent<KeycardPanel>().accessGranted = true;	
				}
			}
			else
			{
				useKeycardText.SetActive(false);
			}


			//if you find an unconscious body, you can pick it up
			if (interactHit.collider.tag == "Unconscious Body" && canGrab)
			{
				unconsciousBody.SetActive(true);
				if(Input.GetKey (pc.interact))
				{
				bodyHeld = interactHit.collider.gameObject;
				bodyHeld.transform.SetParent(grabBodyLocation);
				canGrab = false;
				bodyHeld.GetComponent<Rigidbody>().useGravity = false;
				bodyHeld.GetComponent<Rigidbody> ().isKinematic = true;
				}
			}
			else
			{
				unconsciousBody.SetActive(false);
			}


			//Storing the hitzone as a target
			if (interactHit.collider.tag == "HitZone") 
				chokeTarget = interactHit.collider.gameObject;
			else
				chokeTarget = null;

			//if you're choking someone and you stop, they will recover and become hostile
			if(chokeTarget){

				chokeText.SetActive(true);
				if((Input.GetKey(pc.alternativeInteract)) && interactHit.collider.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState != PatrolAI.State.Unconscious)
				{	
					pl.isCrouched = false;
					chokeTarget.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Choking;
					chokeTime += 1 * Time.deltaTime;
					if(chokeTime > chokeTimer)
					{
					chokeTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Unconscious;
					chokeTime = 0;
					}
				}
				else if((!Input.GetKey(pc.alternativeInteract)) && chokeTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState == PatrolAI.State.Choking)

					{	
						chokeTime = 0;
						chokeTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Recover;
					}

			}
			else
			{
				chokeText.SetActive(false);
			}
		
		}
		else
		{
			doorText.SetActive(false);
			keycardText.SetActive(false);
			grabText.SetActive(false);
			objecHeldText.SetActive(false);
			unconsciousBody.SetActive(false);
			terminalText.SetActive(false);
			useKeycardText.SetActive(false);
			autohackText.SetActive(false);
			comlinkText.SetActive(false);

		}



























		RaycastHit deviceHit;
		Ray deviceRay = new Ray (transform.position, forward);
		if (Physics.Raycast (deviceRay, out deviceHit, deviceDistance,camLayerMask))
			{
			//Acessing Terminals and hacking guards
			if (inventory.equipState == Inventory.State.HoldDevice)
			{

				//This mode is for interacting with terminals
				if(terminalHackMode)
				{	
					comlinkText.SetActive(false);
					comlinkPercentage.SetActive(false);
					deviceDistance = 3.5f;

					if (deviceHit.collider.tag == "Terminal")
					{
						terminalText.SetActive(true);

						if(inventory.keycardLevelOne)
						{
						useKeycardText.SetActive(true);
						}
						if(inventory.numberOfDecoders > 1)
						{
						autohackText.SetActive(true);
						}

						if (Input.GetKey (pc.use))
						{
							if (deviceHit.collider.gameObject.GetComponent<Terminal> ().hacked == false)
							{
								deviceHit.collider.gameObject.GetComponent<Terminal> ().percentageHacked += 15f * Time.deltaTime;
							}
						}
					}
					else
					{
					terminalText.SetActive(false);
					useKeycardText.SetActive(false);
					autohackText.SetActive(false);
					}

					if(Input.GetKeyUp(pc.comMode))
					{
						comHackMode = true;
						terminalHackMode = false;
					}

				}

				//This mode is for hacking the communications link of the guards
				if(comHackMode)
				{
					comHackTimer -= 1* Time.deltaTime;
					deviceDistance = 40f;

					if(comHackTimer < 0 || numberOfOrders < 1)
					{
						guardHacked = null;
						comHackTimer = 30;
					}

					if(guardHacked)
					{


						if(deviceHit.collider.tag == "Floor")
						{
							if(Input.GetKey(pc.use))
									{
									guardHacked.GetComponent<PatrolAI>().agent.destination = deviceHit.collider.gameObject.transform.position;
									guardHacked.GetComponent<PatrolAI>().aiCurrentState = PatrolAI.State.FollowOrder;
									Debug.Log("Ordered");
									numberOfOrders -= 1;
									canHackGuard = true;
									}
						}
							
						if(deviceHit.collider.tag == "Terminal")
						{
							if(Input.GetKey(pc.use))
								{
								if(guardHacked.GetComponent<PatrolAI>().keycardLevel == deviceHit.collider.GetComponent<KeycardPanel>().keycardLevelRequired)
								{
								guardHacked.GetComponent<PatrolAI>().agent.destination = deviceHit.collider.gameObject.transform.position;
								guardHacked.GetComponent<PatrolAI>().aiCurrentState = PatrolAI.State.UnlockDoor;
								}
								numberOfOrders -= 1;
								canHackGuard = true;
								}
						}

					}
					

					if(deviceHit.collider.tag == "Guard")
					{
						
						stringHack = deviceHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked.ToString();
						comlinkPercentage.GetComponentInChildren<Text>().text = stringHack;

						if(deviceHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked <= 100)
						{
							comlinkText.SetActive(true);
							comlinkPercentage.SetActive(true);
							if (Input.GetKey (pc.use))
							{
								deviceHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked += 15 * Time.deltaTime;

								if(deviceHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked >= 100f)
								{
									guardHacked = deviceHit.collider.gameObject;
									numberOfOrders = 1;
									canHackGuard = false;
								}


							}
						}
						
					}
					else
					{
						comlinkText.SetActive(false);
						comlinkPercentage.SetActive(false);
					}


					if(Input.GetKeyUp(pc.hackMode))
					{
						comHackMode = false;
						terminalHackMode = true;

					}
				}
			}
			else
			{
				terminalText.SetActive(false);
				autohackText.SetActive(false);
				comlinkText.SetActive(false);

			}


	}

}
}
