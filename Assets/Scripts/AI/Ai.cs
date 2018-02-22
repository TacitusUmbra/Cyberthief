using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour 
{

	private UnityEngine.AI.NavMeshAgent agent;
	private UnityEngine.AI.NavMeshPath path;

	[Header("AI States")]
	public State aiCurrentState;
	public State defaultState = State.Idle;

	[Header("AI Seeing")]
	public SightSensor sightSensor;
	public SightRay sightRay;

	[Header("AI Hearing")]
	public RunHearingSensor runHearingSensor;
	public float distanceRequiredToHear;
	Vector3 previousCorner;
	public Vector3 target;

	[Header("AI Movement")]
	public Transform[] points;
	public Transform idlePosition;
	public Transform idleTarget;
	public float calmSpeed;
	public float hostileSpeed;
	public float hostileTimer;


	public enum State 
	{
		Idle,
		WalkToIdle,
		Hostile,
		Investigate
	}


	void Start () 
	{
		aiCurrentState = defaultState;
		path = new UnityEngine.AI.NavMeshPath();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		hostileTimer = 0;

	}


	void Update ()
	{
		
		switch (this.aiCurrentState)
		{
		case State.Idle:
			this.Idle ();
			break;
		case State.Hostile:
			this.Hostile ();
			break;
		case State.Investigate:
			this.Investigate ();
			break;
		case State.WalkToIdle:
			this.WalkToIdle ();
			break;
			
			
		}
	
	}

	void Idle()
	{
		transform.LookAt (idleTarget);
		if (sightSensor.isSeeingPlayer == true && sightRay.raySight == true)
			{
				this.aiCurrentState = State.Hostile;
			}
		else if(runHearingSensor.isHearingPlayerRun == true)
			{
				this.aiCurrentState = State.Investigate;
			}
	}

	void WalkToIdle()
	{

		agent.speed = calmSpeed;

		agent.destination = idlePosition.transform.position;
		hostileTimer =0;
		if (sightSensor.isSeeingPlayer == true && sightRay.raySight == true)
		{
			this.aiCurrentState = State.Hostile;
		}
		 if (gameObject.transform.position.x == idlePosition.transform.position.x)
		{
			this.aiCurrentState = State.Idle;
		}	else if (runHearingSensor.isHearingPlayerRun == true)
		{
			this.aiCurrentState = State.Investigate;
		}		
	}

	void Investigate(){
		agent.speed = calmSpeed;


		target = runHearingSensor.hsTarget;

		UnityEngine.AI.NavMesh.CalculatePath(transform.position, target, UnityEngine.AI.NavMesh.AllAreas, path);
			previousCorner = path.corners[0];

		float distance = 0;

			foreach(Vector3 corner in path.corners)
			{
				distance += Vector3.Distance(previousCorner, corner);
			}
			if(distance <= distanceRequiredToHear)
			{
				agent.destination = target;
			}


		if (sightSensor.isSeeingPlayer == true && sightRay.raySight == true)
		{
			this.aiCurrentState = State.Hostile;
		}
		else if(gameObject.transform.position.x == target.x)
		{
			hostileTimer += 1 * Time.deltaTime;
			if (hostileTimer >= 5f)
			{
				runHearingSensor.isHearingPlayerRun = false;
				this.aiCurrentState = State.WalkToIdle;
			}
		}
	}

	void Hostile()
	{
		agent.speed = hostileSpeed;

		target = sightSensor.sfTarget.transform.position;

		agent.destination = target;
		if(sightRay.raySight==false)
		{
			hostileTimer += 1 * Time.deltaTime;
			if (hostileTimer >= 5f)
			{
				runHearingSensor.isHearingPlayerRun = false;
				this.aiCurrentState = State.WalkToIdle;
			}
		}
	}	


	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
			aiCurrentState = State.Hostile;

	}
		
//		if (target)
//		{
//			agent.destination = target.transform.position;
//		} else if (!agent.pathPending && agent.remainingDistance < 0.5f) 
//		{
//			Patrol ();
//		}
//
//		if (sightSensor.isSeeingPlayer == false)
//		{
//			target = null;		
//		}
//		if (sightSensor.isSeeingPlayer == true)
//		{
//			target = ;		
//		}
//		if(runHearingSensor.isHearingPlayerRun == true )
//		{
//						Investigate();
//
//		}

//	void Patrol() 
//	{
//		if (points.Length == 0)
//			return;
//		
//		agent.destination = points[destPoint].position;
//		destPoint = (destPoint + 1) % points.Length;
//	}
//
//	void OnTriggerStay(Collider other)
//	{
//
//		if (other.gameObject.tag == "Player" && attackTimer > 2)
//		{
//			other.gameObject.GetComponent<Health>().health -= 1f;
//			attackTimer = 0f;
//		}
//
//	}
//
//	
//	}
//	}

//

}