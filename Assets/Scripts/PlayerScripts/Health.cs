using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float health;
	public Slider slider;
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
	
	}
}
