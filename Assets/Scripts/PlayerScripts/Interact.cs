using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	//the player config goes here
	public PlayerConfig pc;
	//the raycast distance of the interact ray
	public float interactionDistance;
	//player goes here
	public Player pl;
	//the ineteract layer mask
	public LayerMask interactLayerMask;
	//the inventory goes here
	public Inventory inventory;
	
	//The takedown target
	[Header("Takedown")]
	public GameObject takedownTarget;

	//these are the variables for holding objects
	[Header("Holding Objects")]
	public bool canGrab;
	public GameObject objectHeld;
	public float grabSpeed;
	public Transform grabLocation;
	public float thrust;
	//these are the variables for holding bodies
	[Header("Holding Bodies")]
	public GameObject bodyHeld;
	public Transform grabBodyLocation;
	public float grabBodySpeed;
	public float bodyThrust;
	//these are the texts for interactions
	[Header("Interactions")]
	public GameObject doorText;
	public GameObject keycardText;
	public GameObject pickupObjectText;
	public GameObject throwAndDropText;
	public GameObject takedownText;
	public GameObject IncapacitatedBodyText;
	public GameObject useKeycardText;
	public GameObject throwBodyText;
	public GameObject lightswitchText;


	void Start () 
	{
		//canGrab is true
		canGrab = true;
		//these texts are all inactive
		doorText.SetActive(false);
		keycardText.SetActive(false);
		pickupObjectText.SetActive(false);
		throwAndDropText.SetActive(false);
		throwBodyText.SetActive (false);

	}
	
	void Update ()
	{
		//If you're holding a body, you can throw it by clicking the Use button. the text to throw the body is active. 
		if (bodyHeld)
		{
			throwBodyText.SetActive (true);
			//the object state is Held
			bodyHeld.gameObject.GetComponent<GrabbableObject> ().objectState = GrabbableObject.State.Held;
			//move the body that is held to the grab body location
			bodyHeld.transform.position = Vector3.Lerp (bodyHeld.transform.position, grabBodyLocation.position, grabBodySpeed * Time.deltaTime);
			//the rotation set for the body held
			bodyHeld.transform.rotation = grabBodyLocation.transform.rotation;

			//if the player presses the use key while holding a body, the rigidbody's kinematic is false, the bodyheld is given force forward,
			// the canGrab becomes true, the bodyheld has gravity, the bodyself parent is null, the bodyheld's Grabbable Object State is Dropped
			// and the bodyheld is null. The body is being thrown here.
			if (Input.GetKeyDown (pc.use))
			{
				bodyHeld.GetComponent<Rigidbody> ().isKinematic = false;
				bodyHeld.GetComponent<Rigidbody> ().AddForce (transform.forward * bodyThrust, ForceMode.Impulse);
				canGrab = true;
				bodyHeld.GetComponent<Rigidbody> ().useGravity = true;
				bodyHeld.transform.parent = null;
				bodyHeld.gameObject.GetComponent<GrabbableObject> ().objectState = GrabbableObject.State.Dropped;
				bodyHeld = null;
			}
		}
		else
		{
			//this text is inactive
			throwBodyText.SetActive (false);
		}

		//The interact ray direction is forward
		Vector3 forward = transform.TransformDirection (Vector3.forward);
		//the interact ray hit
		RaycastHit interactHit;
		//the interact ray
		Ray interactRay = new Ray (transform.position, forward);

		// if the interact ray is active, many conditions below will be applied if it hits
		if (Physics.Raycast (interactRay, out interactHit, interactionDistance,interactLayerMask))
		{
			//if the interact ray hits a lightswitch, the player will be given the option to turn off and on the light if they press
			//the interact key
			if (interactHit.collider.tag == "Lightswitch")
			{
				lightswitchText.SetActive(true);
				 if (Input.GetKeyDown (pc.interact))
                {
                    if (interactHit.collider.gameObject.GetComponent<LightSwitch> ().turnedOn)
                    {						
						lightswitchText.GetComponent<Text>().text = "Lightswitch \n[E] Turn On";
                        interactHit.collider.gameObject.GetComponent<LightSwitch> ().turnedOn = false;
                    }
                    else
                        {
							lightswitchText.GetComponent<Text>().text = "Lightswitch \n[E] Turn Off";
                            interactHit.collider.gameObject.GetComponent<LightSwitch> ().turnedOn = true;

                        }
                }
			}
			else
			{
			lightswitchText.SetActive(false);

			}

			//If conditions are met, the player will be able to pick up an object tagged Grabbable if they press the interact key
			//and certain texts will be active
			if (interactHit.collider.tag == "Grabbable" && canGrab && interactHit.collider.gameObject.GetComponent<GrabbableObject>().objectState == GrabbableObject.State.Grounded)
			{
				pickupObjectText.SetActive(true);
				pickupObjectText.GetComponent<Text>().text = "Bottle \n[E] Grab";

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
				//the text is inactive
				pickupObjectText.SetActive(false);				
			}


			//If you're holding something and there is an object being held, have it fall into your hands.
			if(objectHeld)
			{
				//The text is active
				throwAndDropText.SetActive(true);
				//the objectHeld State becomes Held
				objectHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Held;
				//this brings the objectHeld to the grabLocation
				objectHeld.transform.position = Vector3.Lerp (objectHeld.transform.position, grabLocation.position, grabSpeed * Time.deltaTime);
				//if the object hits anything while being held, it will fall with these conditions
				if(objectHeld.gameObject.GetComponent<GrabbableObject>().hit == true)
					{
						canGrab = true;
						objectHeld.GetComponent<Rigidbody>().useGravity = true;
						objectHeld.transform.parent = null;
						objectHeld.gameObject.GetComponent<GrabbableObject>().objectState = GrabbableObject.State.Dropped;
						objectHeld = null;
					}
			//If holding an object, and the player presses the use key, the object will be thrown	
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
				//the textis inactive
				throwAndDropText.SetActive(false);
			}
			

			// If the interact ray hits something tagged door and they press the interact key, but the door is unlocked, it will 
			//change the doorState to Opening
			if (interactHit.collider.tag == "Door")
			{
				doorText.SetActive(true);				
				if(interactHit.collider.gameObject.GetComponent<Door> ().locked == false)
				{
					doorText.GetComponent<Text>().text = "Door \n[E] Open";
					if(Input.GetKeyDown (pc.interact))
					{
						interactHit.collider.gameObject.GetComponent<Door> ().doorState = Door.State.Opening;
					}
				}
				else
				{
					doorText.GetComponent<Text>().text = "Door (Locked)";
				}	

			}
			else
			{
				//the text is inactive
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

			//Picking up a keycard of a particular level and destroying it afterwards, but only if you havne't picked up
			//a keycard already that is of that particular level
			if(interactHit.collider.tag == "Keycard")
			{
				keycardText.SetActive(true);
				
					if(interactHit.collider.gameObject.GetComponent<Keycard>().keycardLevel == 1 && inventory.keycardLevelOne == false)
					{
						keycardText.GetComponent<Text>().text = "Keycard Lv1 \n[E] Pickup";
						if(Input.GetKey(pc.interact))
						{
							inventory.keycardLevelOne = true;
							Destroy(interactHit.collider.gameObject);
						}

					}
					if(interactHit.collider.gameObject.GetComponent<Keycard>().keycardLevel == 2 && inventory.keycardLevelOne == false)
					{								
						keycardText.GetComponent<Text>().text = "Keycard Lv2 \n[E] Pickup";
						if(Input.GetKey(pc.interact))
						{				
							inventory.keycardLevelTwo = true;
							Destroy(interactHit.collider.gameObject);
						}
					}
				
			}
			else
			{
				//the text is inactive
				keycardText.SetActive(false);
			}

			//Using Keycard on Keycard Panel
			if(interactHit.collider.tag == "Terminal")
			{
					if(interactHit.collider.gameObject.GetComponent<KeycardPanel>().keycardLevelRequired == 1 && inventory.keycardLevelOne == true)
					{
						useKeycardText.SetActive(true);

						if(Input.GetKey(pc.interact))
						{
							interactHit.collider.gameObject.GetComponent<KeycardPanel>().accessGranted = true;
						}
					}	
					else if(interactHit.collider.gameObject.GetComponent<KeycardPanel>().keycardLevelRequired == 2 && inventory.keycardLevelTwo == true)
						{
							useKeycardText.SetActive(true);

							if(Input.GetKey(pc.interact))
							{
								interactHit.collider.gameObject.GetComponent<KeycardPanel>().accessGranted = true;	
							}
						}
						
				
			}
			else
			{
				useKeycardText.SetActive(false);
			}


			//if you find an Incapacitated body, you can pick it up
			if (interactHit.collider.tag == "Incapacitated Body" && canGrab)
			{
				IncapacitatedBodyText.SetActive(true);
				IncapacitatedBodyText.GetComponent<Text>().text = "Body \n[E] Pickup";

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
				IncapacitatedBodyText.SetActive(false);
			}


			//Storing the Drone as a target
			if (interactHit.collider.tag == "Drone")
				takedownTarget = interactHit.collider.gameObject;
			else
				takedownTarget = null;

			//If you can perform a takedown, the text will be active and if you press the alternativeInteract key while the Ai isn't hostile or
			//going to the alarm, you will perform a takedown, changing the AI state to Incapacitated
			if(takedownTarget){
				takedownText.SetActive(true);
				if((Input.GetKey(pc.alternativeInteract)) && interactHit.collider.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState != PatrolAI.State.Incapacitated && interactHit.collider.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState != PatrolAI.State.Hostile )
				{	
					takedownTarget.gameObject.GetComponentInParent<PatrolAI>().aiCurrentState = PatrolAI.State.Incapacitated;
				}

			}
			else
			{
				takedownText.SetActive(false);
			}
		
		}
		else
		{
			//these texts are inactive
			doorText.SetActive(false);
			keycardText.SetActive(false);
			pickupObjectText.SetActive(false);
			throwAndDropText.SetActive(false);
			IncapacitatedBodyText.SetActive(false);
			useKeycardText.SetActive(false);
			takedownText.SetActive(false);
		}

	}
}
