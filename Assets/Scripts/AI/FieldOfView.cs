using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public bool playerIsInFieldOfView;
	public GameObject sfTarget;

	void Start()
	{
		playerIsInFieldOfView = false;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playerIsInFieldOfView = true;
			sfTarget = other.gameObject;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{

			playerIsInFieldOfView = false;
		}
	}

}
