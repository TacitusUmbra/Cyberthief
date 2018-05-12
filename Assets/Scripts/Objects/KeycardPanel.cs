using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardPanel : MonoBehaviour {
	//The keycard level required
	public float keycardLevelRequired;
	//The door associated with this keycard panel
	public GameObject door;
	//Bool on whether access is granted or not
	public bool accessGranted;

	// Use this for initialization
	void Start () {

		//accessGranted is false
		accessGranted = false;
	}
	
	// Update is called once per frame
	void Update () {
			// If accessGranted is true, the door associated with this keycard panel is unlocked
			if(accessGranted)
			door.gameObject.GetComponent<Door>().locked = false;
			
	
	}
}

