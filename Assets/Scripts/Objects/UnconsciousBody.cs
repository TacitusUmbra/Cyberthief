using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnconsciousBody : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		gameObject.AddComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
		gameObject.AddComponent<GrabbableObject>();

		gameObject.tag = "Unconscious Body";
	}
}
