using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {

	public bool broken;
	// Use this for initialization
	void Start () {
		broken = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Breakable"){
			broken = true;
			gameObject.SetActive (false);
			Destroy (other.gameObject);
	}}

}
