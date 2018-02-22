using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent agent;
	private UnityEngine.AI.NavMeshPath path;

	public bool touchAlarm;
	public Transform alarmLocation;

	[Header("AI States")]
	public State aiCurrentState;
	public State defaultState = State.Patrol;

	public State aiCurrentEmotionalState;
	public State defaultEmotionalState;

	[Header("AI Hearing")]
	public RunHearingSensor runHearingSensor;
	public float distanceRequiredToHear;
	Vector3 previousCorner;
	public Vector3 target;

	[Header("AI Seeing")]
	public SightSensor sightSensor;
	public SightRay sightRay;

	[Header("AI Movement")]
	public Transform[] points;
	private int destPoint = 0;
	public float calmSpeed;
	public float calmSpeedTimer;
	public float hostileSpeed;
	public float hostlileSpeedTimer;
	public float hostileTimer;
	public float hostileVigilanceTimer;
	public float stressedSpeed;

	[Header("AI Suspicion")]
	public float distanceToPlayer;
	public float suspicionAmount;
	public float suspicionCap;
	public float suspicionGrowth;
	public float suspicionStart;
	public float suspicionRate;

	[Header("AI Stress")]
	public float levelOfStress;
	public float stressShrinkValue;
	public float stressGrowthValue;
	public float stressedEntryLevel;
	public float stressedExitLevel;
	public float paranoidEntryLevel;
	public float paranoidExitLevel;


	public enum State 
	{
		Patrol,
		Hostile,
		Investigate,
		Suspicion,
		Alerted,
		Calm,
		Stressed,
		Paranoid,
		GoToAlarm
	}

	void Start () 
	{
		aiCurrentState = defaultState;
		path = new UnityEngine.AI.NavMeshPath();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		hostileTimer = 0;
		aiCurrentEmotionalState = defaultEmotionalState;

	}


	void Update ()
	{


		switch (this.aiCurrentState)
		{
	
		case State.Hostile:
			this.Hostile ();
			break;
		case State.Investigate:
			this.Investigate ();
			break;
		case State.Patrol:
			this.Patrol ();
			break;
		case State.Alerted:
			this.Alerted ();
			break;
		case State.Suspicion:
			this.Suspicion ();
			break;
		case State.GoToAlarm:
			this.GoToAlarm ();
			break;
			
		}
		switch (this.aiCurrentEmotionalState)
		{

		case State.Calm:
			this.Calm ();
			break;
		case State.Stressed:
			this.Stressed ();
			break;
		case State.Paranoid:
			this.Paranoid ();
			break;

		}

			
	}

	void Patrol()
	{
		agent.speed = Mathf.Lerp (agent.speed, calmSpeed, hostlileSpeedTimer * Time.deltaTime);

        agent.destination = points[destPoint].position;
		hostileTimer =0;

		if(gameObject.transform.position.x == points[destPoint].position.x)
		{
			if(gameObject.transform.position.z == points[destPoint].position.z )
			{
				destPoint = (destPoint + 1) % points.Length;
				Patrol();
			}
		}


		if (sightSensor.isSeeingPlayer && sightRay.raySight && sightRay.target.GetComponent<Player>().visibility > 40f)
			{
			this.aiCurrentState = State.Suspicion;
			}
		 else if (runHearingSensor.isHearingPlayerRun)
			{
				this.aiCurrentState = State.Alerted;
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
				this.aiCurrentState = State.Patrol;
			}
		}
	}




	void Hostile()
	{
		//Suspicion becomes 0
		suspicionAmount = 0;
		//Have the AI begin to run from walking
		agent.speed = Mathf.Lerp (agent.speed, hostileSpeed, calmSpeedTimer * Time.deltaTime);
		//target is the position of the player
		target = sightSensor.sfTarget.transform.position;
		//have the AI run after the player
		agent.destination = target;
		//if the AI can no longer see the player, begin counting until five seconds have elapsed. In that case
		//go back to patrol because you have lost the player
		if(!sightRay.raySight)
		{
			hostileTimer += 1 * Time.deltaTime;
			if (hostileTimer >= hostileVigilanceTimer)
			{
				runHearingSensor.isHearingPlayerRun = false;
				this.aiCurrentState = State.Patrol;
			}
		}
	}

	void GoToAlarm()
	{
		agent.destination = alarmLocation.transform.position;
		//the Ai will run to the alarm and upon touching it, they will return to a stressed emotional state
		if (touchAlarm)
		{
			levelOfStress = 60;
			this.aiCurrentEmotionalState = State.Stressed;
			this.aiCurrentState = State.Patrol;
			touchAlarm = false;
			Debug.Log ("Patrolling");
		}
	}
		
	void Suspicion()
	{


		if (sightSensor.isSeeingPlayer && sightRay.raySight && sightRay.target.GetComponent<Player>().visibility > 40f)
		{
			distanceToPlayer = Vector3.Distance(sightSensor.sfTarget.transform.position, transform.position);
			suspicionGrowth = suspicionRate / distanceToPlayer;

			levelOfStress += stressGrowthValue * Time.deltaTime;
			suspicionAmount += suspicionGrowth * Time.deltaTime;
			if (suspicionAmount >= suspicionCap)
				aiCurrentState = State.Hostile;
			
		} else{
			suspicionAmount -= suspicionGrowth * Time.deltaTime;
			if (suspicionAmount <= suspicionStart)
				aiCurrentState = State.Patrol;
		}
	}

	void Alerted()
	{
		
		//Run Animation, then to check what made a sound
		//After the AI is alerted, it will investigate
		Debug.Log("What was that?!");
		levelOfStress = levelOfStress + 20f;
		this.aiCurrentState = State.Investigate;

	}

	void Calm()
	{
		//if calm, the suspicion cap becomes three
		suspicionCap = 3;
		//if they reach the level of stress needed, they enter the emotional state of being stressed
		if (levelOfStress > stressedEntryLevel)
			aiCurrentEmotionalState = State.Stressed;
		//if the level of stress if above zero, the level of stress will lower itself over time
		if(levelOfStress > 0)
			levelOfStress -= stressShrinkValue * Time.deltaTime;
	}

	void Stressed()
	{
		agent.speed = Mathf.Lerp (agent.speed, stressedSpeed, calmSpeedTimer * Time.deltaTime);
		levelOfStress -= stressShrinkValue * Time.deltaTime;
		suspicionCap = 2;

		if (levelOfStress < stressedExitLevel)
			aiCurrentEmotionalState = State.Calm;
		else if (levelOfStress > paranoidEntryLevel)
		{
			aiCurrentEmotionalState = State.Paranoid;
		}
	}

	void Paranoid()
	{
		agent.speed = Mathf.Lerp (agent.speed, hostileSpeed, calmSpeedTimer * Time.deltaTime);
		levelOfStress -= stressShrinkValue * Time.deltaTime;
		aiCurrentState = State.GoToAlarm;
		stressShrinkValue = 0.5f;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
			aiCurrentState = State.Hostile;
		if (other.gameObject.tag == "Alarm" && aiCurrentState == State.GoToAlarm)
		{
			Debug.Log ("Touching");
			touchAlarm = true;
		}
	}
}