using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {
	//Switch State
	public bool switchState;
	// The light source
	public GameObject lightSource;
	// The intensity of the light
	public float intensityOfLight;
	//the bool determining if the light is turned on
	public bool turnedOn;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{	
		// If switchState, the light intensity of the lightsource is the intensity of light we decide
		//Otherwise, the lightsource's intensity is zero
		if (switchState)
		{
			lightSource.GetComponent<Light> ().intensity = intensityOfLight;
		}else
		{
			lightSource.GetComponent<Light> ().intensity = 0f;

		}
	}



}
