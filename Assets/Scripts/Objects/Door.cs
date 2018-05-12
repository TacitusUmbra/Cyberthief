using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	
	//Door animations
	public Animation doorAnim;
	//Door State
	public State doorState;
	//Default Door Dtate
	public State defaultdoorState = State.Closed;
	//Bool on whether the door is locked
	public bool locked;
	//GameObject to the closed lights
	public GameObject closedLights;
	//GameObject to the open Lights
	public GameObject openLights;


	//The States of the Door
	public enum State 
	{
		Open,
		Closed,
		Closing,
		Opening
	}
	void Start() 
	{
		//the door State is the default door state
		doorState = defaultdoorState;
	}
	void Update() 
	{
		//all the door states
		switch (this.doorState)
		{

		case State.Open:
			this.Open ();
			break;
		case State.Opening:
			this.Opening ();
			break;
		case State.Closed:
			this.Closed ();
			break;
		case State.Closing:
			this.Closing ();
			break;

		}

		//if locked, some lights will be active and others not
		if(locked)
		{
			closedLights.SetActive(true);
			openLights.SetActive(false);

		}
		//if not locked, some lights will be active and others not
		if(!locked)
		{
			closedLights.SetActive(false);
			openLights.SetActive(true);
		}

	}
	//Open funtion
	void Open()
	{
		
	}
	//Closed function
	void Closed()
	{
		
	}

	//Opening function where teh animation is played to open the door
	void Opening()
	{	
		
		doorAnim.Play ("Industrial Door Open");
		doorState = State.Open;
		
	}
	// Closing function where the animation is played to close the door
	void Closing()
	{
		
		doorAnim.Play ("Industrial Door Close");
		doorState = State.Closed;
		
	}
		
}

