using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	//Health goes here
	public Health hl;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//the slider value is the health value
		gameObject.GetComponent<Slider>().value = hl.gameObject.GetComponent<Health>().health;
		
	}
}
