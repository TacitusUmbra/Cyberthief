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

	[Header("Holding")]
	public bool canGrab;
	public GameObject objectHeld;
	public float grabSpeed;
	public Transform grabLocation;
	public float thrust;
	void Start () 
	{
		canGrab = true;
	}
	
	void Update ()
	{
		



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

			//Acessing Terminals to hack them
			if (interactHit.collider.tag == "Terminal" && Input.GetKey(pc.interact))
			{
				if(interactHit.collider.gameObject.GetComponent<Terminal>().hacked == false)
				{
					interactHit.collider.gameObject.GetComponent<Terminal>().percentageHacked += 15f * Time.deltaTime;
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




			}
		}

		
	
	
}
