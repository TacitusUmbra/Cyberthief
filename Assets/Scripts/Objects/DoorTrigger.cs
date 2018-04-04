using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	public Door door;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player" && door.doorState == Door.State.Open)
		{
			door.doorState = Door.State.Closing;
		}
		
	}

	void OnTriggerEnter(Collider enter)
	{
		if(enter.gameObject.tag == "Guard" )
			{

			}
	}

}
