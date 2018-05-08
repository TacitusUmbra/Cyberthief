using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComHack : MonoBehaviour {

	public PlayerConfig pc;
	public Inventory inventory;

	public float comlinkDistance;
	public float numberOfOrders;
	public float comHackTimer;

	public GameObject comlinkText;
	public GameObject terminalText;
	public GameObject comlinkPercentage;
	public GameObject guardHacked;
	public GameObject autohackText;

	public bool canHackGuard;

	public string stringHack;

	public LayerMask comlinkLayer;

	// Use this for initialization
	void Start () 
	{
		comlinkDistance = 40f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (inventory.equipState == Inventory.State.HoldComDevice)
		{
		Vector3 forward = transform.TransformDirection (Vector3.forward);
		RaycastHit comlinkHit;
		Ray comlinkRay = new Ray (transform.position, forward);
		if (Physics.Raycast (comlinkRay, out comlinkHit, comlinkDistance,comlinkLayer))
				{

				terminalText.SetActive (false);
				autohackText.SetActive (false);

					if(comHackTimer < 0 || numberOfOrders < 1)
					{
						guardHacked = null;
					}

					if(guardHacked)
					{
						comHackTimer = 30;

						comHackTimer -= 1 * Time.deltaTime;

						if(comlinkHit.collider.tag == "Floor")
						{
							if(Input.GetKey(pc.use))
									{
									guardHacked.GetComponent<PatrolAI>().agent.destination = comlinkHit.collider.gameObject.transform.position;
									guardHacked.GetComponent<PatrolAI>().aiCurrentState = PatrolAI.State.FollowOrder;
									Debug.Log("Ordered");
									numberOfOrders -= 1;
									canHackGuard = true;
									}
						}
							
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
					

					if(comlinkHit.collider.tag == "Guard" && comlinkHit.collider.gameObject.GetComponent<PatrolAI>().aiCurrentState != PatrolAI.State.Hostile && comlinkHit.collider.gameObject.GetComponent<PatrolAI>().aiCurrentState != PatrolAI.State.GoToAlarm)
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

								if(comlinkHit.collider.gameObject.GetComponent<PatrolAI>().comlinkPercentageHacked >= 100f)
								{
									guardHacked = comlinkHit.collider.gameObject;
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



				}
				else
				{
				terminalText.SetActive(false);
				comlinkText.SetActive(false);
				}	
		}
	}
			


	
	
}
