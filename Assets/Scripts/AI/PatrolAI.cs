using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent agent;
	
	//the set alarm Location
	public Transform alarmLocation;
	//The vector of the previous corner used in checking a path
	Vector3 previousCorner;
	//The interactzones, hearing trigger and field of view trigger childed to the AI
	public GameObject interactZones;
	//the Ai's idle spot
	public GameObject idleSpot;
	//The AI's idle look at, where it will look when idle
	public Transform IdleLookAt;
	//The Ai's lights
	public GameObject droneLights;
	//AI rotating speed
	public float rotationSpeed;
	//The distance between it and the ordered location
	public float distanceToOrderedLocation;
	//The distance required to the ordered location
	public float distanceRequiredToOrderedLocation;
	//The AI keycard Level
	public float keycardLevel;
	//This is to control the material color of the lights on the AI
	private Material lightColor;
	//The wait timer of the AI
	public float waitTimer;
	//The cooldown for the AI to wait
	public float waitCooldown;
	//Determining whether the AI is an idleguard or patrolguard
	public bool idleGuard;
	public bool patrolGuard;
	//The Player's Player script
	public Player pl;
	
	//AI State variables
	[Header("AI States")]
	public State aiCurrentState;
	public State defaultState;
	public State aiCurrentEmotionalState;
	public State defaultEmotionalState = State.Calm;
	


	//Variables related to the AI's 
	[Header("AI Hearing")]
	public Hearing aiHearing;
	public Vector3 hearingTarget;
	public float distanceRequiredToHear;
	public float distanceToSound;
	public float investigateTimer;
	public float timeWillingToInvestigate;
	public float withinRangeOfSound;

	//Variables related to the AI's sight
	[Header("AI Seeing")]
	public FieldOfView Fov;
	public Vector3 locationOfPlayerPreviouslySighted;

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
	//The timer and cooldown for the AI to shoot
	[Header("AI Shooting Cooldown")]
	public float shootTimer;
	public float shootCooldown;
	//The percentage which the AI is hacked and the bool whether it was hacked or not
	[Header("ComLink")]
	public float comlinkPercentageHacked;
	public bool comlinkHacked; 


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
		Incapacitated,
		Idle,
		WalkBackToIdleSpot,
		FollowOrder,
		UnlockDoor,
		Wait
	}

	void Start () 
	{
		//If the AI is a patrol guard, it will change its state to patrol. If it is an idle guard, it will have its state be Idle.
		if(patrolGuard)
		{
		defaultState = State.Patrol;
		}
		else if(idleGuard)
		{
		defaultState = State.Idle;
		}
		//The current state of the ai is the default state
		aiCurrentState = defaultState;
		// Getting the navmesh agent
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//Setting the hostile timer to zero
		hostileTimer = 0;
		// The current emotional state of the Ai is set to the default emotional state
		aiCurrentEmotionalState = defaultEmotionalState;
		//The comlink has not been hacked
		comlinkHacked = false;
		//This line is to control the lights that are around the drone
		lightColor = droneLights.GetComponent<Renderer>().material;
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
		case State.Incapacitated:
			this.Incapacitated ();
			break;
		case State.Idle:
			this.Idle ();
			break;
		case State.WalkBackToIdleSpot:
			this.WalkBackToIdleSpot ();
			break;
		case State.FollowOrder:
			this.FollowOrder ();
			break;
		case State.UnlockDoor:
			this.UnlockDoor();
			break;
		case State.Wait:
			this.Wait();
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
		//If the AI can see a body, it becomes Paranoid
		if(Fov.canSeeBody)
		{
			aiCurrentEmotionalState = State.Paranoid;
		}
		//The speed of the Ai will slowly lerp towards the calm speed
		agent.speed = Mathf.Lerp (agent.speed, calmSpeed, hostlileSpeedTimer * Time.deltaTime);
		//The destination of the AI is the destination point's position
        agent.destination = points[destPoint].position;
		//Hostile Timer becomes zero
		hostileTimer =0;
		//When it is at the x and z position of its destination point, it will continue to patrol
		if(gameObject.transform.position.x == points[destPoint].position.x)
		{
			if(gameObject.transform.position.z == points[destPoint].position.z )
			{
				destPoint = (destPoint + 1) % points.Length;
				Patrol();
			}
		}
		//If the AI can see the player, by fulfilling these conditions, it will become Hostile and set the shootimer to 2.9
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.sightTarget.GetComponent<Player>().visibility > 40f)
		{
			shootTimer = 2.9f;
			this.aiCurrentState = State.Hostile;
		}
		//If the AI cannot really see the player, as determined by the visibility, but other conditions are met, 
		//it will change its current state to suspicion
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.sightTarget.GetComponent<Player>().visibility < 40f &&  Fov.sightTarget.GetComponent<Player>().visibility > 20f)
			{
			this.aiCurrentState = State.Suspicion;
			}
			//if the AI can hear something, it becomes Alerted
		else if (aiHearing.canHearSomething)
			{
				this.aiCurrentState = State.Alerted;
			}
			//If the emotional State of the AI is calm and it was actually and Idle Guard, it will walk back to it's idle spot
		else if((aiCurrentEmotionalState == State.Calm) && idleGuard == true)
		{
			this.aiCurrentState = State.WalkBackToIdleSpot;
		}
				
	}

	void Idle()
	{
		//If the AI can see a body, it becomes Paranoid
		if(Fov.canSeeBody)
			{
				aiCurrentEmotionalState = State.Paranoid;
			}
		//These lines of code will have the AI look in the direction of its IdleLookAt's position when it is idle,
		//meaning it will be looking in a particular direction when Idle
 		Vector3 targetDir = IdleLookAt.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
		targetDir.y =0;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
		//If the AI can see the player, by fulfilling these conditions, it will become Hostile and set the shootimer to 2.9
		if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.sightTarget.GetComponent<Player>().visibility > 40f)
			{
				shootTimer = 2.9f;
			this.aiCurrentState = State.Hostile;
			}
			//If the AI cannot really see the player, as determined by the visibility, but other conditions are met, 
			//it will change its current state to suspicion
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.sightTarget.GetComponent<Player>().visibility < 40f &&  Fov.sightTarget.GetComponent<Player>().visibility > 20f)
			{
			this.aiCurrentState = State.Suspicion;
			}
			//If the Ai can hear something, it will change the current state to Alerted
		else if (aiHearing.canHearSomething)
			{
				this.aiCurrentState = State.Alerted;
			}

	}

	//These are the conditions for the AI when it is walking back to idle spot
	void WalkBackToIdleSpot()
	{
		//The destination of the AI is the idle spot
		agent.destination = idleSpot.transform.position;

		//If the AI can see a body, it becomes Paranoid
		if(Fov.canSeeBody)
			{
				aiCurrentEmotionalState = State.Paranoid;
			}

		//If the AI can see the player, by fulfilling these conditions, it will become Hostile and set the shootimer to 2.9
		if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.sightTarget.GetComponent<Player>().visibility > 40f)
		{
			shootTimer = 2.9f;
			this.aiCurrentState = State.Hostile;
		}

		//If the AI cannot really see the player, as determined by the visibility, but other conditions are met, 
		//it will change its current state to suspicion
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.sightTarget.GetComponent<Player>().visibility < 40f &&  Fov.sightTarget.GetComponent<Player>().visibility > 20f)
			{
			this.aiCurrentState = State.Suspicion;
			}

		//If the Ai can hear something, it will change the current state to Alerted
		else if (aiHearing.canHearSomething)
			{
				this.aiCurrentState = State.Alerted;
			}
			//If the Ai is at the x and z position of it's idle spot, it will change it's current state to Idle
		else if(transform.position.x == idleSpot.transform.position.x)
			{
				if(transform.position.z == idleSpot.transform.position.z)
				{
				this.aiCurrentState = State.Idle;
				}
			}
	}

//The state prior tothe investigative state where the AI may perform an animation before investigating what made a sound.
	void Alerted()
	{
		//After the AI is alerted, it will investigate
		this.aiCurrentState = State.Investigate;

	}

//The State where the AI will investigate a sound it heard
	void Investigate()
	{
		//The speed of the AI is calm speed
		agent.speed = calmSpeed;
		//The hearing target of the AI is the hearing target of its hearing trigger
		hearingTarget = aiHearing.hearingTarget;
		//The destination of the AI becomes the hearing target 
		agent.destination = hearingTarget;
		//The distance to sound that the AI will use to determine when it is close to where the hearing target was
		distanceToSound = Vector3.Distance(hearingTarget, transform.position);

		//If the AI can see the player, by fulfilling these conditions, it will become Hostile and set the shootimer to 2.9
		if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.sightTarget.GetComponent<Player>().visibility > 40f)
		{
			shootTimer = 2.9f;
			this.aiCurrentState = State.Hostile;
		}

		//If the AI cannot really see the player, as determined by the visibility, but other conditions are met, 
		//it will change its current state to suspicion
		else if (Fov.playerInFieldOfView && Fov.canSeePlayer &&  Fov.sightTarget.GetComponent<Player>().visibility < 40f &&  Fov.sightTarget.GetComponent<Player>().visibility > 20f)
		{
			this.aiCurrentState = State.Suspicion;
		}
		//If the Ai is within the distance to where it believed the sound was heard, the investigate Timer will increase.
		//Once the investigate timer is greater than the time willing to investigate the timer, it will either change it's state to
		//Patrol if it was a patrol guard or WalkBackToIdleSpot if it was an idle guard.
		else if(distanceToSound <= withinRangeOfSound)
		{
			//Investigate Timer increases
			investigateTimer += 1 * Time.deltaTime;
			//If conditions are met, it will change its current state to Patrol
			if ((investigateTimer >= timeWillingToInvestigate) && patrolGuard == true)
			{
				investigateTimer = 0;
				aiHearing.canHearSomething = false;
				aiHearing.couldHearSomething = false;
				this.aiCurrentState = State.Patrol;
			}
			//If conditions are met, it will change it's current state to WalkBackToIdleSpot. However, if it is stressed,
			//the Ai will change its current state to Patrol
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
		shootTimer += 1 * Time.deltaTime;
		//Suspicion becomes 0
		suspicionAmount = 0;

		//Have the AI's speed will become it's hostile speed
		agent.speed = Mathf.Lerp (agent.speed, hostileSpeed, calmSpeedTimer * Time.deltaTime);

		//If it can see the player, the locationOfPlayerPreviouslySighted is where the AI last saw the player and it will rotate
		//in order to continue looking at the player
		if(Fov.canSeePlayer)
		{
		locationOfPlayerPreviouslySighted = Fov.sightTarget.transform.position;

		Vector3 playerDirection = Fov.sightTarget.transform.position - transform.position;
		float hostileTurning = rotationSpeed * Time.deltaTime;
		playerDirection.y = 0f;
		Vector3 newPlayerDirection = Vector3.RotateTowards(transform.forward, playerDirection, hostileTurning, 0.0F);
        transform.rotation = Quaternion.LookRotation(newPlayerDirection);
			//It will reduce the player health when the shoot timer is greater than the shoot cooldown
			if(shootTimer > shootCooldown)
			{
				Fov.sightTarget.GetComponent<Health>().health -=1f;
				shootTimer = 0;
			}

		}		

		//If the AI can no longer see the player, begin counting until the hostile timer is greater than the hostile vigilance timer.
		// In that case go back to patrol because you have lost the player
		if (!Fov.canSeePlayer)
		{
			agent.ResetPath ();
			agent.SetDestination (locationOfPlayerPreviouslySighted);

			Debug.Log (agent.pathStatus);
			Debug.DrawLine (transform.position, locationOfPlayerPreviouslySighted);
			
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

	//The state where the AI will run to the alarm and set it in order to alert other guards in the scene.
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
		//This will stop the player when they are in the state
		agent.isStopped = true;
		//If the Player is sighted, determine the distance from the player to the AI
		if(Fov.sightTarget)
		distanceToPlayer =  Vector3.Distance(Fov.sightTarget.position, transform.position);
		//These set the suspicion cap based on the distance to the player
		if (distanceToPlayer > 10f)
			suspicionCap = 3f;
		if (distanceToPlayer < 10f)
			suspicionCap = 2f;
		if ( distanceToPlayer < 5f)
			suspicionCap = 1f;
			//If the AI can see the player, by fulfilling these conditions, it will become Hostile and set the shootimer to 2.9
		if (Fov.playerInFieldOfView && Fov.canSeePlayer && Fov.sightTarget.GetComponent<Player>().visibility > 40f)
		{
			shootTimer = 2.9f;
			this.aiCurrentState = State.Hostile;
		}
		//If the AI can see the player, it will begin becoming stressed and it's suspicion amount will increase
		else if (Fov.canSeePlayer)
		{
			levelOfStress += stressGrowthValue * Time.deltaTime;
			suspicionAmount += suspicionGrowth * Time.deltaTime;
		}
		//if the AI cannot see the player, it will change the current state to Patrol. The suspicion amount will decrease over time.
		else if(!Fov.canSeePlayer)
		{
			suspicionAmount -= suspicionGrowth * Time.deltaTime;
			if (suspicionAmount <= suspicionStart)
			{
				agent.isStopped = false;
				aiCurrentState = State.Patrol;
			}
		}
		//If the suscpicion amount is greater than the suspicion cap, the AI's shoot timer is set to 2.9, it can move once more,
		//and it's current state is Hostile
		if (suspicionAmount >= suspicionCap)
		{
			shootTimer = 2.9f;
			agent.isStopped = false;
			aiCurrentState = State.Hostile;
		}

	}
	
	void Calm()
	{
		// The lights on the AI will become green
		 lightColor.SetColor("_EmissionColor", Color.green);

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
		// The lights on the AI will become yellow
		 lightColor.SetColor("_EmissionColor", Color.yellow);
		// The speed of the Ai will change to it's stressed speed
		agent.speed = Mathf.Lerp (agent.speed, stressedSpeed, calmSpeedTimer * Time.deltaTime);
		//The level of stress will decrease over time based on the stress shrink value
		levelOfStress -= stressShrinkValue * Time.deltaTime;
		//The suspicion cap will be 2
		suspicionCap = 2;
		//If the level of stress is lower than the stressed exit level, the AI's current emotional state will be calm
		if (levelOfStress < stressedExitLevel)
		{
			aiCurrentEmotionalState = State.Calm;
		}
		//if hte level of stress is greater than the paranoid entry level, the AI's current emotional state will be paranoid
		else if (levelOfStress > paranoidEntryLevel)
		{
			aiCurrentEmotionalState = State.Paranoid;
		}
	}

	void Paranoid()
	{
		//The lights on the AI will become red
         lightColor.SetColor("_EmissionColor", Color.red);
		// The speed of the Ai will become the hostile speed
		agent.speed = Mathf.Lerp (agent.speed, hostileSpeed, calmSpeedTimer * Time.deltaTime);
		//The level of stress will decreased based on teh stress shrink value over time
		levelOfStress -= stressShrinkValue * Time.deltaTime;
		//if there is an alarm given to the Ai, it will change it's current State to GoToAlarm, which causes it to go to an alarm
		if(alarmLocation)
		{
		aiCurrentState = State.GoToAlarm;
		}
		//the AI can no longer hear something
		aiHearing.canHearSomething = false;
		//The Ai could no longer hear something
		aiHearing.couldHearSomething = false;
		//the stress shrink value is set to 0.25
		stressShrinkValue = 0.25f;
		//if the level of stress is lower than the paranoid Exit level, the current emotional state is stressed
		if(levelOfStress < paranoidExitLevel)
		{
		aiCurrentEmotionalState = State.Stressed;
		}

	}

	void Incapacitated()
	{
		// The nav mesh agent on the AI is false
		gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		//the interact zones, hearing and sight, are destroyed
		Destroy(interactZones);
		//The component, IncapacitatedBody is added to the AI
		gameObject.AddComponent<IncapacitatedBody>();
		//This script is destroyed
		Destroy(this);
	}

	//The AI will follow an order and move to a particular location designated by the player
	void FollowOrder()
	{
		//The distance to the ordered location relative to the AI position
		distanceToOrderedLocation = Vector3.Distance(agent.destination,transform.position);
		//If the distance to ordered location is less than the distance required to ordered location, the ai current state will be Wait
		if(distanceToOrderedLocation < distanceRequiredToOrderedLocation)
		{
			aiCurrentState = State.Wait;
		}
	}
	//The AI will walk to the designated location if the player orders the AI to unlock a door.
	void UnlockDoor()
	{	//The distance to the ordered location relative to the AI position
		distanceToOrderedLocation = Vector3.Distance(agent.destination,transform.position);
		//If the distance to ordered location is less than the distance required to ordered location, the ai current state will be Wait
		if(distanceToOrderedLocation < distanceRequiredToOrderedLocation)
		{
		aiCurrentState = State.Wait;
		}
	}
	//The AI will wait 
	void Wait()
	{	
		//The wait timer will increase over time
		waitTimer += 1 * Time.deltaTime;
		//if the wait timer is greater than the wait cooldown and idleGuard is true, the Ai's currentState will become WalkBackToIdleSpot
		if(waitTimer > waitCooldown && idleGuard == true)
		{
			aiCurrentState = State.WalkBackToIdleSpot;
		}
		//if the wait timer is greater than the wait cooldown and patrolGuard is true, the Ai's currentState will become Patrol
		if(waitTimer > waitCooldown && patrolGuard == true)
		{
			aiCurrentState = State.Patrol;
		}

	}

	void OnTriggerEnter (Collider other)
	{	
		//If the Player touches the AI, it will set the shoot timer to 2.9 and it will change the current state to Hostile
		if (other.gameObject.tag == "Player")
		{
			locationOfPlayerPreviouslySighted = pl.gameObject.transform.position;
			shootTimer = 2.9f;
			aiCurrentState = State.Hostile;
		}
			
	}
}