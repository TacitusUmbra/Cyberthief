using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	//health float
	public float health;
	//The slider goes here
	public Slider slider;
	//The respawn zone
	public Vector3 respawnZone;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		//if the health is above 5, keep it at 5
		if (health > 5)
		{
			health = 5;
		}
		// if the health is below 1, change the position of the gameObject to be the respawnZone position and health becomes 5
		if(health < 1)
		{
			gameObject.transform.position = respawnZone;
			health = 5;
		}
	
	}
}
