using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	

	public Animation doorAnim;

	public State doorState;
	public State defaultdoorState = State.Closed;
	public bool locked;

	public GameObject closedLights;
	public GameObject openLights;



	public enum State 
	{
		Open,
		Closed,
		Closing,
		Opening
	}
	void Start() 
	{
		doorState = defaultdoorState;
	}
	void Update() 
	{

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

		if(locked)
		{
			closedLights.SetActive(true);
			openLights.SetActive(false);

		}
		
		if(!locked)
		{
			closedLights.SetActive(false);
			openLights.SetActive(true);
		}

	}

	void Open()
	{
		
	}
	void Closed()
	{
		
	}
	void Opening()
	{	
		
		doorAnim.Play ("Industrial Door Open");
		doorState = State.Open;
		
	}
	void Closing()
	{
		
		doorAnim.Play ("Industrial Door Close");
		doorState = State.Closed;
		
	}
		
}

