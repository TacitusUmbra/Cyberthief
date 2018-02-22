using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardPanel : MonoBehaviour {

	public float keycardLevelRequired;
	public GameObject Door;
	public bool accessGranted;

	// Use this for initialization
	void Start () {
		accessGranted = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(accessGranted)
			Door.gameObject.GetComponent<Door>().locked = false;
		
	}
}
