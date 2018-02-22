using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSensor : MonoBehaviour {

	public bool isSeeingPlayer;
	public GameObject sfTarget;

	void Start()
	{
		isSeeingPlayer = false;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			isSeeingPlayer = true;
			sfTarget = other.gameObject;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{

			isSeeingPlayer = false;
		}
	}

}
