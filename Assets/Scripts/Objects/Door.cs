using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	

	public Animation doorAnim;

	public State doorState;
	public State defaultdoorState = State.Closed;
	public bool locked;



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


		if (Input.GetKeyDown(KeyCode.Backspace)){
			locked = false;
		}

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
	}

	void Open()
	{
		
	}
	void Closed()
	{
		
	}
	void Opening()
	{	
		
		doorAnim.Play ("Open Door");
		doorState = State.Open;
		
	}
	void Closing()
	{
		
		doorAnim.Play ("Close Door");
		doorState = State.Closed;
		
	}
		
}

