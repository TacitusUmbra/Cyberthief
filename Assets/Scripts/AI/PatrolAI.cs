 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent agent;
	
	//the set alarm Location
	public Transform alarmLocation;
	Vector3 previousCorner;
	public GameObject hitters;
	public GameObject idleSpot;
	public Transform IdleLookAt;
	public float rotationSpeed;

	public bool idleGuard;
	public bool patrolGuard;
	
	//AI State variables
	[Header("AI States")]
	public State aiCurrentState;
	public State defaultState;
	public State aiCurrentEmotionalState;
	public State defaultEmotionalState = State.Calm;
	


	//Variables related to the AI's hearing
	[Header("AI Hearing")]
	public Hearing aiHearing;
	public Vector3 target;
	public float distanceRequiredToHear;
	public float distanceToSound;
	public float investigateTimer;
	public float timeWillingToInvestigate;
	public float withinRangeOfSound;

	//Variables related to the AI's sight
	[Header("AI Seeing")]
	public FieldOfView Fov;

	//Variables related to the AI's movement
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

	//Variables related to the AI's suspicion
	[Header("AI Suspicion")]
	public float distanceToPlayer;
	public float suspicionAmount;
	public float suspicionCap;
	public float suspicionGrowth;
	public float suspicionStart;
	public float suspicionRate;

	//Variables related to the AI's stress
	[Header("AI Stress")]
	public float levelOfStress;
	public float stressShrinkValue;
	public float stressGrowthValue;
	public float stressedEntryLevel;
	public float stressedExitLevel;
	public float paranoidEntryLevel;
	public float paranoidExitLevel;

	//Ai States
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
		GoToAlarm,
		Choking,
		Unconscious,
		Recover,
		Idle,
		WalkBackToIdleSpot
	}

	void Start () 
	{
		if(patrolGuard)
		{
		defaultState = State.Patrol;
		}
		else if(idleGuard)
		{
		defaultState = State.Idle;
		}

		aiCurrentState = defaultState;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		hostileTimer = 0;
		aiCurrentEmotionalState = defaultEmotionalState;
	}


	void Update ()
	{

		//The AI's states
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
		case State.Choking:
			this.Choking ();
			break;
		case State.Unconscious:
			this.Unconscious ();
			break;
		case State.Recover:
			this.Recover ();
			break;
		case State.Idle:
			this.Idle ();
			break;
		case State.WalkBackToIdleSpot:
			this.WalkBackToIdleSpot ();
			break;
			
		}

		//The AI's emotional states
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

	//The AI's patrol state where they will walk between points unless some conditions are met to switch states, in which they can become suspicious, hostile, or alerted.
	void Patrol()
	{
		if(Fov.canSeeBody)
		{
			aiCurrentEmotionalState = State.Paranoid;
		}

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
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.target.GetComponent<Player>().visibility < 40f)
			{
			this.aiCurrentState = State.Suspicion;
			}
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.target.GetComponent<Player>().visibility > 40f)
			{
			this.aiCurrentState = State.Hostile;
			}
		else if (aiHearing.canHearSomething)
			{
				this.aiCurrentState = State.Alerted;
			}
		else if((aiCurrentEmotionalState == State.Calm) && idleGuard == true)
		{
			this.aiCurrentState = State.WalkBackToIdleSpot;
		}
				
	}

	void Idle()
	{
		if(Fov.canSeeBody)
			{
				aiCurrentEmotionalState = State.Paranoid;
			}

 		Vector3 targetDir = IdleLookAt.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
		targetDir.y =0;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

		if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.target.GetComponent<Player>().visibility < 40f)
			{
			this.aiCurrentState = State.Suspicion;
			}
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.target.GetComponent<Player>().visibility > 40f)
			{
			this.aiCurrentState = State.Hostile;
			}
		else if (aiHearing.canHearSomething)
			{
				this.aiCurrentState = State.Alerted;
			}


	}
	void WalkBackToIdleSpot()
	{
		agent.destination = idleSpot.transform.position;

		if(Fov.canSeeBody)
			{
				aiCurrentEmotionalState = State.Paranoid;
			}

		if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.target.GetComponent<Player>().visibility < 40f)
			{
			this.aiCurrentState = State.Suspicion;
			}
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.target.GetComponent<Player>().visibility > 40f)
			{
			this.aiCurrentState = State.Hostile;
			}
		else if (aiHearing.canHearSomething)
			{
				this.aiCurrentState = State.Alerted;
			}
		else if(transform.position.x == idleSpot.transform.position.x)
			{
				if(transform.position.z == idleSpot.transform.position.z)
				{
				this.aiCurrentState = State.Idle;
				}
			}
	}

//The state prior tothe investigative state where the AI will perform an animation before investigating what made a sound.
	void Alerted()
	{
		//Run Animation, then to check what made a sound
		//After the AI is alerted, it will investigate
		Debug.Log("What was that?!");
		levelOfStress = levelOfStress + 20f;
		this.aiCurrentState = State.Investigate;

	}


	void Investigate()
	{
		agent.speed = calmSpeed;
		target = aiHearing.hearingTarget;
		agent.destination = target;
		distanceToSound = Vector3.Distance(target, transform.position);

		if (Fov.playerInFieldOfView == true && Fov.canSeePlayer == true)
		{
			this.aiCurrentState = State.Hostile;
		}
		
		else if(distanceToSound <= withinRangeOfSound)
		{
			investigateTimer += 1 * Time.deltaTime;
			if ((investigateTimer >= timeWillingToInvestigate) && patrolGuard == true)
			{
				investigateTimer = 0;
				aiHearing.canHearSomething = false;
				aiHearing.couldHearSomething = false;
				this.aiCurrentState = State.Patrol;
			}
			else if((investigateTimer >= timeWillingToInvestigate) && idleGuard == true)
			{
				if(levelOfStress < stressedEntryLevel)
				{
				investigateTimer = 0;
				aiHearing.canHearSomething = false;
				aiHearing.couldHearSomething = false;
				this.aiCurrentState = State.WalkBackToIdleSpot;
				}
				else if((idleGuard == true) && levelOfStress > stressedEntryLevel)
				{
				aiHearing.canHearSomething = false;
				aiHearing.couldHearSomething = false;
				aiCurrentState = State.Patrol;
				}
			}
		}
		
	}


	//The AI's state where they are hostile and running after the player. The AI's speed will change, and they will make their destination the player's position. 
	//If they no longer can see the player for ten seconds, they will go back to patrol.
	void Hostile()
	{
		//Suspicion becomes 0
		suspicionAmount = 0;

		//Have the AI begin to run from walking
		agent.speed = Mathf.Lerp (agent.speed, hostileSpeed, calmSpeedTimer * Time.deltaTime);

		//target is the position of the player
		target = Fov.target.transform.position;

		//have the AI run after the player
		agent.destination = target;

		//if the AI can no longer see the player, begin counting until five seconds have elapsed. In that case
		//go back to patrol because you have lost the player
		if (!Fov.canSeePlayer)
		{
			hostileTimer += 1 * Time.deltaTime;
			if (hostileTimer >= hostileVigilanceTimer)
			{
				aiHearing.canHearSomething = false;
				this.aiCurrentState = State.Patrol;
			}
		} else if (Fov.canSeePlayer && hostileTimer >= 0f)
		{
			hostileTimer = 0;
		}
	}

	//The state where the AI will run to the alarmand set it in order to alert other guards in the scene.
	void GoToAlarm()
	{
		agent.destination = alarmLocation.transform.position;
		if(transform.position.x == alarmLocation.transform.position.x)
		{
			if(transform.position.z == alarmLocation.transform.position.z)
			{
			levelOfStress = 60;
			this.aiCurrentEmotionalState = State.Stressed;
			this.aiCurrentState = State.Patrol;				
			}
		}		
	}
		
	//The state where the AI can 'see' the player in shadow, but they must wait for a particular amount of time before becoming hostile.
	void Suspicion()
	{
		agent.isStopped = true;
		distanceToPlayer =  Vector3.Distance(Fov.target.position, transform.position);

		if (distanceToPlayer > 10f)
			suspicionCap = 3f;
		if (distanceToPlayer < 10f)
			suspicionCap = 2f;
		if ( distanceToPlayer < 5f)
			suspicionCap = 1f;

		if (Fov.canSeePlayer)
		{
			levelOfStress += stressGrowthValue * Time.deltaTime;
			suspicionAmount += suspicionGrowth * Time.deltaTime;
		}
		else if(!Fov.canSeePlayer)
		{
			suspicionAmount -= suspicionGrowth * Time.deltaTime;
			if (suspicionAmount <= suspicionStart)
			{
				agent.isStopped = false;
				aiCurrentState = State.Patrol;
			}
		}
		if (suspicionAmount >= suspicionCap)
		{
			agent.isStopped = false;
			aiCurrentState = State.Hostile;
		}

	}

	void Choking()
	{
		agent.isStopped = true;
		
	}

	void Recover()
	{
		agent.isStopped = false;
		aiCurrentState = State.Hostile;
	}

	void Calm()
	{
		//if calm, the suspicion cap becomes three
		suspicionCap = 3;
		//Bring the stress shrink value to this value
		stressShrinkValue = 0.5f;
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
		{
			aiCurrentEmotionalState = State.Calm;
		}
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
		aiHearing.canHearSomething = false;
		aiHearing.couldHearSomething = false;
		stressShrinkValue = 0.25f;
	}

	void Unconscious()
	{
		gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		Destroy(hitters);
		gameObject.AddComponent<UnconsciousBody>();
		Destroy(this);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
			aiCurrentState = State.Hostile;
	}
}