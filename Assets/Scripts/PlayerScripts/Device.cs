using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Device : MonoBehaviour {
	// player config goes here
	public PlayerConfig pc;
	//inventory goes here
	public Inventory inventory;
	//the device layer mask
	public LayerMask deviceLayer;
	//terminal text goes here
	public GameObject terminalText;
	//autohack text goes here
	public GameObject autohackText;
	//comlink text goes here
	public GameObject comlinkText;
	//comlink percentage goes here
	public GameObject comlinkPercentage;
	//device percentage goes here
	public GameObject devicePercentage;
	//the distance of the device ray
	public float deviceDistance;
	//string for showing numbers
	private string stringHack;

	


// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if the inventory equipState is HoldDevice, the comlink text and percentage of inactive
		if (inventory.equipState == Inventory.State.HoldDevice)
		{

			comlinkText.SetActive (false);
			comlinkPercentage.SetActive (false);
			//the direction of the ray for the device
			Vector3 forward = transform.TransformDirection (Vector3.forward);
			//the hit of the ray for the device
			RaycastHit deviceHit;
			//the new ray of the device
			Ray deviceRay = new Ray (transform.position, forward);
			//the raycast of the device
			if (Physics.Raycast (deviceRay, out deviceHit, deviceDistance,deviceLayer))
				{
					//device distance
					deviceDistance = 1.5f;
					//if the ray hits something tagged terminal,it will check if the percentage hacked is less than 100
					//if so, the devicePercentage and terminalText will be active. If there is more than one numeber of decoders
					//in the inventory, the player will see the autohack text
					if (deviceHit.collider.tag == "Terminal")
					{
						if(deviceHit.collider.gameObject.GetComponent<Terminal> ().percentageHacked < 100)
							{
							devicePercentage.SetActive(true);
							terminalText.SetActive(true);

							stringHack = deviceHit.collider.gameObject.GetComponent<Terminal>().percentageHacked.ToString();
							devicePercentage.GetComponentInChildren<Text>().text = stringHack;
							
							if(inventory.numberOfDecoders > 1)
								{
								autohackText.SetActive(true);
								}

							}
							//if they aren't hitting the terminal, the text will all become inactive
						else
						{
						devicePercentage.SetActive(false);
						terminalText.SetActive(false);
						autohackText.SetActive(false);
						}
						
						//if they press the use key and the terminal isn't hacked, they will begin hacking the terminal, increasing the
						//percentage hacked over time
						if (Input.GetKey (pc.use))
						{
							if (deviceHit.collider.gameObject.GetComponent<Terminal> ().hacked == false)
							{
								deviceHit.collider.gameObject.GetComponent<Terminal> ().percentageHacked += 15f * Time.deltaTime;
							}
						}
					}
					//otherwise these text are inactive
					else
					{
					terminalText.SetActive(false);
					autohackText.SetActive(false);
					devicePercentage.SetActive(false);

					}

				}
				//otherwise, these text are inactive
				else
					{
					terminalText.SetActive(false);
					autohackText.SetActive(false);
					devicePercentage.SetActive(false);

					}
		}

	}
}
