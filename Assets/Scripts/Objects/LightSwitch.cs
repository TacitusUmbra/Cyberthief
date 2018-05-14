using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {	
	// The light source
	public GameObject lightSource;
	// The intensity of the light
	public float intensityOfLight;
	//the bool determining if the light is turned on
	public bool turnedOn;
	//the lightswitch button
	public GameObject lightswitchButton;
	//the material of the button
	private Material buttonLight;

	// Use this for initialization
	void Start ()
	{
		buttonLight = lightswitchButton.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		// If switchState, the light intensity of the lightsource is the intensity of light we decide
		//Otherwise, the lightsource's intensity is zero
		if (turnedOn)
		{
			lightSource.GetComponent<Light> ().intensity = intensityOfLight;
			buttonLight.SetColor("_EmissionColor", Color.green);

		}else
		{
			lightSource.GetComponent<Light> ().intensity = 0f;
			buttonLight.SetColor("_EmissionColor", Color.red);

		}
	}



}
