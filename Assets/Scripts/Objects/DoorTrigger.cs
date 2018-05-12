using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	//The door associated with the door trigger goes here
	public Door door;
	//The terminal associated with the door trigger is put here
	public Terminal terminal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//if the player leaves the trigger and the door's State is Open, it will close the door with an animation
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player" && door.doorState == Door.State.Open)
		{
			door.doorState = Door.State.Closing;
		}
		
	}
	//If a Keycard enters the trigger zone, used for ordering AI to unlock doors, the door will become unlocked
	void OnTriggerEnter(Collider enter)
	{
		if(enter.gameObject.tag == "Keycard" )
			{
			if(terminal)
				{
				if (enter.GetComponent<Keycard> ().keycardLevel == terminal.GetComponent<KeycardPanel>().keycardLevelRequired)
					{
					door.GetComponent<Door> ().locked = false;
					}
				}
			}
	}


}
