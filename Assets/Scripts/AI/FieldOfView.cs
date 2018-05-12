using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	// Variables for the field of view of the AI
	public bool canSeePlayer;
	public bool canSeeBody;
	public bool playerInFieldOfView;
	public bool bodyInFieldOfView;
	public float sightDistance;
	public LayerMask sightLayer;
	public Transform sightTarget;


	void Start()
	{
		// At the beginning, it cannot see the player or a body
		canSeePlayer = false;
		canSeeBody = false;
	}
	
	// If a Player or Unconscious Body is outside of the field of view, it cannot be potentially seen by the AI
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerInFieldOfView = false;
			canSeePlayer = false;
		}
		if(other.gameObject.tag == "Unconscious Body")
		{
			bodyInFieldOfView = false;
			canSeeBody = false;
		}
		
	}
	
	//Conditions upon entering the collider of the field of view
	void OnTriggerStay(Collider checkPosition)
	{
		//When something enters the field of view that is a player or an Unconscious Body, it will send a raycast in that direction to
		//determine if the object is in light of sight. If it can see one of the two, it will be change the variables to true, otherwise, they are false.
		Vector3 forward = checkPosition.transform.position - this.transform.position;
		RaycastHit sightHit;
		Ray sightRay = new Ray (transform.position, forward);

		if ((checkPosition.gameObject.tag == "Player"))
		{
			playerInFieldOfView = true;

			if (Physics.Raycast (sightRay, out sightHit, sightDistance,sightLayer))
			{		
				if((sightHit.collider.tag == "Player"))
				{
				sightTarget = sightHit.collider.gameObject.transform;
				canSeePlayer = true;
				}
				else 
				{
				canSeePlayer=false;
				sightTarget = null;
				}
					
			}
			
		}
		
		if ((checkPosition.gameObject.tag == "Unconscious Body" && checkPosition.gameObject.GetComponent<IncapacitatedBody>().seen == false))
		{
			bodyInFieldOfView = true;

			if (Physics.Raycast (sightRay, out sightHit, sightDistance,sightLayer))
			{		
				if((sightHit.collider.tag == "Unconscious Body"))
				{
				checkPosition.gameObject.GetComponent<IncapacitatedBody>().seen = true;
				canSeeBody = true;
				}
				else 
				{
					canSeeBody=false;
				}
				
				
			}
			
		}
		
	}
	
}
