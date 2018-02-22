using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightRay : MonoBehaviour
{
	public float sightRayDistance;
	public Transform target;
	public bool raySight;
	public SightSensor ss;

	// Use this for initialization
	void Start () 
	{

		raySight = false;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ss.isSeeingPlayer)
		{
			transform.LookAt (target);
			Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;

			RaycastHit sightHit;
			Ray sightRay = new Ray (transform.position, forward);
			if (Physics.Raycast (sightRay, out sightHit, sightRayDistance))
			{		
				if (sightHit.collider.tag == "Player")
					raySight = true;
				else
				{
					raySight = false;
				}


			}

		
		}
	}
		
}

