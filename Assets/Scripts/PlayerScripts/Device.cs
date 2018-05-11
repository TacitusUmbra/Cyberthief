using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Device : MonoBehaviour {

	public PlayerConfig pc;
	public Inventory inventory;
	public LayerMask deviceLayer;
	public GameObject terminalText;
	public GameObject autohackText;
	public GameObject comlinkText;
	public GameObject comlinkPercentage;
	public GameObject devicePercentage;
	public float deviceDistance;
	private string stringHack;

	


// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (inventory.equipState == Inventory.State.HoldDevice)
		{

			comlinkText.SetActive (false);
			comlinkPercentage.SetActive (false);

			Vector3 forward = transform.TransformDirection (Vector3.forward);
			RaycastHit deviceHit;
			Ray deviceRay = new Ray (transform.position, forward);
			if (Physics.Raycast (deviceRay, out deviceHit, deviceDistance,deviceLayer))
				{
				//Acessing Terminals and hacking guards
				
					deviceDistance = 1.5f;

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
						else
						{
						devicePercentage.SetActive(false);
						terminalText.SetActive(false);
						autohackText.SetActive(false);
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
					autohackText.SetActive(false);
					devicePercentage.SetActive(false);

					}

				}
				else
					{
					terminalText.SetActive(false);
					autohackText.SetActive(false);
					devicePercentage.SetActive(false);

					}
		}

	}
}
