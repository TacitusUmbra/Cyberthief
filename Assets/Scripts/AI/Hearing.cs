using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {

	public bool isHearingPlayer;
	public bool heardSomething;
	public float distanceForHeight;
	public float distanceRequiredToHear;
	public Vector3 hearingTarget;
	
	// Use this for initialization
	void Start () 
	{
		isHearingPlayer = false;
		heardSomething = false;

	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run )
		{
			isHearingPlayer = true;
			hearingTarget = other.transform.position;
		}

		//To fix afterwards
		/*else if (other.gameObject.tag == "Breakable" && other.gameObject.GetComponent<Throwing>().crashed == true)
		{

			isHearingPlayerRun = true;
			hsTarget = other.transform.position;

		}*/

			

	}

		

			
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			isHearingPlayer = false;


	}



		


}
	
	

	

