using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour {

	public GameObject emitter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			emitter.SetActive(true);
		}
	}

	void OnTriggerExit(Collider another)
	{
		if(another.gameObject.tag == "Player")
		{
			emitter.SetActive(false);
		}
	}
}
