using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour {


	public float distanceRequiredToHear;
	public float distanceForHeight;


	public bool canHearSomething;
	public Vector3 hearingTarget;
	Vector3 previousCorner;
	public GameObject soundToInstantiate;

	// Use this for initialization
	void Start () 
	{
		canHearSomething = false;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run)
		{
			canHearSomething = true;
			hearingTarget = other.transform.position;
		}

	
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			canHearSomething = false;
	}
}
	
	

	

