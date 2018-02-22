using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

	public bool switchState;
	public GameObject lightSource;
	public GameObject visibilityBox;
	public float intensityOfLight;
	public bool turnedOn;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (lightSource.GetComponent<Light> ().intensity == intensityOfLight)
			visibilityBox.SetActive (true);
		else
			visibilityBox.SetActive (false);
			


		gameObject.GetComponent<Renderer>().material.color = Color.white;

		if (switchState)
		{
			lightSource.GetComponent<Light> ().intensity = intensityOfLight;
		}else
		{
			lightSource.GetComponent<Light> ().intensity = 0f;

		}
	}



}
