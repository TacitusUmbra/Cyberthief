using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {


	public bool canSeePlayer;
	public bool playerInFieldOfView;
	public bool bodyInFieldOfView;
	public int numberOfThingsInView;
	public float sightDistance;
	public LayerMask sightLayer;
	public Transform target;

	public List <GameObject> thingsInFieldofView = new List<GameObject>();

	void Start()
	{
		canSeePlayer = false;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerInFieldOfView = true;
			numberOfThingsInView =+ 1;
			thingsInFieldofView.Add(other.gameObject);
			
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerInFieldOfView = false;
			numberOfThingsInView =- 1;
			thingsInFieldofView.Remove(other.gameObject);
			canSeePlayer = false;
		}
		
	}
	void Update()
	{
		
	}


	void OnTriggerStay(Collider checkPosition)
	{

		if ((checkPosition.gameObject.tag == "Player") ||(checkPosition.gameObject.tag == "Unconscious Body"))
		{
			Debug.Log("inside");

			Vector3 forward = checkPosition.transform.position - this.transform.position;
			RaycastHit sightHit;
			Ray sightRay = new Ray (transform.position, forward);
			if (Physics.Raycast (sightRay, out sightHit, sightDistance,sightLayer))
			{		
				if((sightHit.collider.tag == "Player"))
				{
				Debug.DrawRay(transform.position, forward, Color.green);
				Debug.Log(sightHit);
				canSeePlayer = true;
				}
				else 
				{
					canSeePlayer=false;
				}
				
				if(sightHit.collider.tag == "Unconscious Body")
				{
					bodyInFieldOfView = true;
				}
			}
			
		}
		
	}
	
}
