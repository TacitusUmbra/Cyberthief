using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardPanel : MonoBehaviour {

	public float keycardLevelRequired;
	public GameObject door;
	public GameObject vent;
	public bool accessGranted;

	// Use this for initialization
	void Start () {
		accessGranted = false;
	}
	
	// Update is called once per frame
	void Update () {


			if(door)
			{
			if(accessGranted)
			door.gameObject.GetComponent<Door>().locked = false;
			}

			if(vent)
			{
			
			}
		
	}
}
