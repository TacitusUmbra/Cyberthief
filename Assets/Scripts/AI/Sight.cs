using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
	public float sightRayDistance;
	public Transform target;
	public bool canSeePlayer;
	public FieldOfView Fov;
	public bool canSeeBody;

	// Use this for initialization
	void Start () 
	{
		canSeePlayer = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit sightHit;
		Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
		Ray sightRay = new Ray (transform.position, forward);
		transform.LookAt (target);


		if (Physics.Raycast (sightRay, out sightHit, sightRayDistance))
		{
			if (Fov.playerIsInFieldOfView)
			{
				if (sightHit.collider.tag == "Player")
				{
					canSeePlayer = true;
				} else
				{
					canSeePlayer = false;
				}

			}
		}
		else if (!Fov.playerIsInFieldOfView)
		{
			canSeePlayer = false;
		}

	}
		
}

