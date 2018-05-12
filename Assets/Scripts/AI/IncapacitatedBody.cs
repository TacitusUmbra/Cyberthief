using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncapacitatedBody : MonoBehaviour {

	//Variable determining if the AI has been seen
	public bool seen;

	// Use this for initialization
	void Start () 
	{
		//When the script is called, it will alter the these components of the AI, who is unconscious, 
		//preparing it to be acted as an Incapacitated Body
		seen = false;
		gameObject.AddComponent<Rigidbody>();
		gameObject.GetComponent<MeshCollider>().isTrigger = false;
		gameObject.AddComponent<GrabbableObject>();
		gameObject.tag = "Incapacitated Body";
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
