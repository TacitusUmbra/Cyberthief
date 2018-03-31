using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {


	public bool canSeePlayer;
	public bool canSeeBody;
	public bool playerInFieldOfView;
	public bool bodyInFieldOfView;
	public float sightDistance;
	public LayerMask sightLayer;
	public Transform sightTarget;


	void Start()
	{
		canSeePlayer = false;
		canSeeBody = false;
	}
	
	
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
	
	void OnTriggerStay(Collider checkPosition)
	{
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
		
		if ((checkPosition.gameObject.tag == "Unconscious Body" && checkPosition.gameObject.GetComponent<UnconsciousBody>().seen == false))
		{
			bodyInFieldOfView = true;

			if (Physics.Raycast (sightRay, out sightHit, sightDistance,sightLayer))
			{		
				if((sightHit.collider.tag == "Unconscious Body"))
				{
				checkPosition.gameObject.GetComponent<UnconsciousBody>().seen = true;
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
