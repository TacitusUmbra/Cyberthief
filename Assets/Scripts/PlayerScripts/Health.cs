using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float health;
	public Slider slider;
	public Transform respawnZone;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {

		if (health > 5)
		{
			health = 5;
		}
		if(health < 1)
		{
			gameObject.transform.position = respawnZone.position;
			health = 5;
		}
	
	}
}
