using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	public Door door;
	public Terminal terminal;

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
		if(enter.gameObject.tag == "Keycard" )
			{
			Debug.Log ("SAD");
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
