using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {

	public bool isHearingPlayer;
	public Vector3 hearingTarget;

	// Use this for initialization
	void Start () 
	{
		isHearingPlayer = false;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run )
		{
			isHearingPlayer = true;
			hearingTarget = other.transform.position;
		}


		else if (other.gameObject.tag == "Grabbable" && other.gameObject.GetComponent<GrabbableObject>().objectState == GrabbableObject.State.Break)
		{

			isHearingPlayer = true;
			hearingTarget = other.transform.position;

		}
		
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			isHearingPlayer = false;


	}



		


}
	
	

	

