using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComHack : MonoBehaviour {
		//player config goes here
	public PlayerConfig pc;
	//inventory goes here
	public Inventory inventory;
	//the comlink distance
	public float comlinkDistance;
	//number of orders 
	public float numberOfOrders;
	//the comhack timer
	public float comHackTimer;
	//the comlink text goes here
	public GameObject comlinkText;
	//the terminal text goes here
	public GameObject terminalText;
	//the comlink percentage text goes here
	public GameObject comlinkPercentage;
	//the guard hacked
	public GameObject guardHacked;
	//the autohack text goes here
	public GameObject autohackText;
	//bool to determine if you can hack a guard
	public bool canHackGuard;
	//string used to have a string
	public string stringHack;
	//layer mask for comlink
	public LayerMask comlinkLayer;

	// Use this for initialization
	void Start () 
	{
		//the comlink distance is 40
		comlinkDistance = 40f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the inventory equipstate is HoldComDevice, 
		if (inventory.equipState == Inventory.State.HoldComDevice)
		{
			//direction of ray for comlink
		Vector3 forward = transform.TransformDirection (Vector3.forward);
		//the hit of the comlink
		RaycastHit comlinkHit;
		//the ray of the comlink
		Ray comlinkRay = new Ray (transform.position, forward);
		//if a ray is active, ther terminal text and autohack are inactive and other conditions are available
		if (Physics.Raycast (comlinkRay, out comlinkHit, comlinkDistance,comlinkLayer))
				{

				terminalText.SetActive (false);
				autohackText.SetActive (false);
					// if the timer is less than 0 or the number of orders is less than 0, the guardhacked is null
					if(comHackTimer < 0 || numberOfOrders < 1)
					{
						guardHacked = null;
					}

					
					//if the guard is hacked, the comHackTimer will begin to decrease over time and other conditions are available
					if(guardHacked)
					{
						comHackTimer -= 1 * Time.deltaTime;
						//if the comlink hits the floor and the player presses the use key, the AI will walk to the location it is 
						//ordered to move to and the number of orders will decrease by one and the canHackGuard becomes true
						if(comlinkHit.collider.tag == "Floor")
						{
							if(Input.GetKey(pc.use))
									{
									guardHacked.GetComponent<PatrolAI>().agent.destination = comlinkHit.collider.gameObject.transform.position;
									guardHacked.GetComponent<PatrolAI>().aiCurrentState = PatrolAI.State.FollowOrder;
									numberOfOrders -= 1;
									canHackGuard = true;
									}
						}
						//if the comlink hits the terminal and the player presses the use key, teh ai will walk to the terminal 
						//and unlock the door if it has a keycard level equivalent to the door. The numberof orders will decrease by one
						//and the canHackGuard becomes true
						if(comlinkHit.collider.tag == "Terminal")
						{
							if(Input.GetKey(pc.use))
								{
									if(guardHacked.GetComponent<PatrolAI>().keycardLevel == comlinkHit.collider.GetComponent<KeycardPanel>().keycardLevelRequired)
									{
										guardHacked.GetComponent<PatrolAI>().agent.destination = comlinkHit.collider.gameObject.transform.position;
										guardHacked.GetComponent<PatrolAI>().aiCurrentState = PatrolAI.State.UnlockDoor;
									}
								numberOfOrders -= 1;
								canHackGuard = true;
								}
						}

					}
					
					//If the comlink ray hits something tagged Drone and it isn't Hostile or going to the alarm, the player may begin to hack
					//the AI if their comlinkPercentageHacked is less than 100. Certain text will become active and if the player presses the
					//use key, they will begin hacking the AI. Once hacked, it will set the guardHacked to the Ai, the comHackTimer will be 30,
					//the number of order will be 1, the canHackGuard will be false, and the AI comlink will be hacked
					if(comlinkHit.collider.tag == "Drone" && comlinkHit.collider.gameObject.GetComponent<PatrolAI>().aiCurrentState != PatrolAI.State.Hostile && comlinkHit.collider.gameObject.GetComponent<PatrolAI>().aiCurrentState != PatrolAI.State.GoToAlarm)
					{
						
						stringHack = comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked.ToString();
						comlinkPercentage.GetComponentInChildren<Text>().text = stringHack;

						if(comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked <= 100)
						{
							comlinkText.SetActive(true);
							comlinkPercentage.SetActive(true);
							if (Input.GetKey (pc.use))
							{
								comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked += 15 * Time.deltaTime;

								if(comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked >= 100f && comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkHacked == false)
								{
									guardHacked = comlinkHit.collider.gameObject;
									comHackTimer = 30;
									numberOfOrders = 1;
									canHackGuard = false;
									comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkHacked = true;
								}


							}
						}
						
					}
					//otherwise, these texts are inactive
					else
					{
						comlinkText.SetActive(false);
						comlinkPercentage.SetActive(false);
					}



				}
				//otherwise, these texts are inactive
				else
				{
				terminalText.SetActive(false);
				comlinkText.SetActive(false);
				}	
		}
	}
			


	
	
}
