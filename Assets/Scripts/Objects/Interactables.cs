using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {
	//bool to see if something is broken
	public bool broken;
	// Use this for initialization
	void Start () {
		//broken is false
		broken = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//If the other object is breakable and broken is true, the gameobject is setactive to false and the other gameobject is destroyed
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Breakable"){
			broken = true;
			gameObject.SetActive (false);
			Destroy (other.gameObject);
	}}

}
