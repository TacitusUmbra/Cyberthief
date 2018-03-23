using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {

	private UnityEngine.AI.NavMeshPath path;

	public float distanceRequiredToHear;
	public float distanceForHeight;
	public bool canHearSomething;
	public Vector3 hearingTarget;
	Vector3 previousCorner;
	public GameObject soundToInstantiate;
	public Vector3 hitArea;
	public Vector3 myposition;
	public bool couldHearSomething;
	public PatrolAI patrol;

	// Use this for initialization
	void Start () 
	{
		canHearSomething = false;
		path = new UnityEngine.AI.NavMeshPath();
		couldHearSomething = false;
	}

	void OnTriggerStay(Collider other)
	{
		UnityEngine.AI.NavMeshHit myNavHit;
		if (other.gameObject.tag == "Grabbable")
		{
			if (other.gameObject.GetComponent<GrabbableObject> ().objectState == GrabbableObject.State.Break)
			{
				if (UnityEngine.AI.NavMesh.SamplePosition (other.gameObject.transform.position, out myNavHit, 50.0f, UnityEngine.AI.NavMesh.AllAreas))
				{
					hitArea = myNavHit.position;
					myposition = transform.position;
					couldHearSomething = true;

					if (couldHearSomething)
					{
						UnityEngine.AI.NavMesh.CalculatePath (transform.position, hitArea, UnityEngine.AI.NavMesh.AllAreas, path);
						previousCorner = path.corners [0];

						float distance = 0;

						foreach (Vector3 corner in path.corners)
						{
							distance += Vector3.Distance (previousCorner, corner);
						}
						if (distance <= distanceRequiredToHear)
						{
							patrol.levelOfStress = patrol.levelOfStress + 20f;
							hearingTarget = hitArea;
							canHearSomething = true;
						} else
						{
							couldHearSomething = false;
							Debug.Log ("Too Far");
						}
					}

				}

			}
		}

		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run)
		{
			UnityEngine.AI.NavMesh.CalculatePath(transform.position, other.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);
			previousCorner = path.corners[0];

			float distance = 0;

			foreach(Vector3 corner in path.corners)
			{
				distance += Vector3.Distance(previousCorner, corner);
			}
			if(distance <= distanceRequiredToHear)
			{
				canHearSomething = true;
				hearingTarget = other.transform.position;
				patrol.aiCurrentEmotionalState = PatrolAI.State.Stressed;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			canHearSomething = false;
	}
		
}
	
	

	

