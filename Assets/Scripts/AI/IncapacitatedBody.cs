using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncapacitatedBody : MonoBehaviour {

	public bool seen;

	// Use this for initialization
	void Start () 
	{
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
