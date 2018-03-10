using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {

	public bool isHearingPlayer;
	public Vector3 hearingTarget;
	public float distanceRequiredToHear;
	private UnityEngine.AI.NavMeshPath path;
	Vector3 previousCorner;
	public float distanceForHeight;
	public GameObject soundToInstantiate;

	// Use this for initialization
	void Start () 
	{
		isHearingPlayer = false;
		path = new UnityEngine.AI.NavMeshPath();

	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run )
		{
			hearingTarget = other.transform.position;
			UnityEngine.AI.NavMesh.CalculatePath(transform.position, hearingTarget, UnityEngine.AI.NavMesh.AllAreas, path);
			previousCorner = path.corners[0];

			float distance = 0;

			foreach(Vector3 corner in path.corners)
			{
				distance += Vector3.Distance(previousCorner, corner);
			}
			if(distance <= distanceRequiredToHear)
			{
			isHearingPlayer = true;
			hearingTarget = other.transform.position;
			}

			
		}


		if (other.gameObject.tag == "Grabbable" && other.gameObject.GetComponent<GrabbableObject>().objectState == GrabbableObject.State.Break)
		{	
			UnityEngine.AI.NavMeshHit navHit;

			UnityEngine.AI.NavMesh.SamplePosition(hearingTarget, out navHit, distanceForHeight, UnityEngine.AI.NavMesh.AllAreas);

			UnityEngine.AI.NavMesh.CalculatePath(transform.position, navHit.position, UnityEngine.AI.NavMesh.AllAreas, path);
			previousCorner = path.corners[0];

			float distance = 0;

			foreach(Vector3 corner in path.corners)
			{
				distance += Vector3.Distance(previousCorner, corner);
			}
			if(distance <= distanceRequiredToHear)
			{
			Debug.Log("I heard something!");

			isHearingPlayer = true;
			hearingTarget = other.transform.position;
			}

		}
		
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			isHearingPlayer = false;


	}



		


}
	
	

	

