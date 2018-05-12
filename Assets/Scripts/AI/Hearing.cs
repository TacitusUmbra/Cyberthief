using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {
	
	//The path that the AI will use to calculate whether something is in an appropriate distance from it
	private UnityEngine.AI.NavMeshPath path;

	//Variables that the AI will use to determine whether it can hear things in the environment
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
		//The AI cannot hear anything, I could not hear anything, and we set the path upon the navmesh
		canHearSomething = false;
		path = new UnityEngine.AI.NavMeshPath();
		couldHearSomething = false;
	}

	//A function that will trigger if anything could be heard from the AI
	void OnTriggerStay(Collider other)
	{
		
		UnityEngine.AI.NavMeshHit myNavHit;

		//If something that is grabbable has its state become Break within the appropriate distance to the AI, the AI will calculate
		//a path and determine if it is within hearing range. If not, it does not hear anything, otherwise, it will hear something.
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
						}
					}

				}

			}
		}

		//If the Player's state is Run, which means they are running, and they are within the trigger, the AI will calculate a path
		//to determine if the Player is within hearing range. If not, the AI will not hear the player. If it can hear the player, 
		//the AI will hear the player and become stressed
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run)
		{
			UnityEngine.AI.NavMesh.CalculatePath(transform.position, other.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);

			if(path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
			{
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
	}
	//If the player leaves the trigger, it can no longer hear the player.
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			canHearSomething = false;
	}
		
}
	
	

	

