using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

	//Bool used to determine if the object has hit something	
	public bool hit;
	//State of objectState
	public State objectState;
	//defaultObjectState is set to Grounded 
	public State defaultObjectState = State.Grounded;
	//this is used to determine the light which is hit
	public GameObject lightHit;
	//The grabbable object's states
	public enum State 
		{
			Grounded,
			Held,
			Thrown,
			Dropped,
			Break
		}

	void Start()
	{
		// objectState is the defaultObjectState
		objectState = defaultObjectState;
		//hit is false
		hit = false;
	}
	void Update()
	{
	//all the states that the grabbable objects may undergo
	switch (this.objectState)
		{
			case State.Grounded:
			this.Grounded();
			break;
			
			case State.Dropped:
			this.Dropped ();
			break;

			case State.Thrown:
			this.Thrown ();
			break;

			case State.Held:
			this.Held ();
			break;

			case State.Break:
			this.Break ();
			break;
		}
	}
	
	//Colliding with other things
	void OnCollisionEnter(Collision collision)
	{
		//if the grabbable object is in teh held State and hits something other than the ground or player, hit is true
		if(objectState == State.Held)
		{
			if(collision.gameObject.tag != "Ground" || collision.gameObject.tag !="Player")
			{
				hit = true;
			}	
		}
		//if the objectState is dropped, and it hits the ground, the state becomes grounded	
		if(objectState == State.Dropped)
		{
			if(collision.gameObject.tag == "Ground")
				{	
					objectState = State.Grounded;
				}

		}
		//if the object state is thrown and it hits a light, it will change the stant to broken and the light hit will become inactive
		if (objectState == State.Thrown)
		{
			if(collision.gameObject.tag == "Light")
			{
				lightHit = collision.gameObject.GetComponentInParent<Lights>().lightObject;
				lightHit.SetActive(false);
				objectState = State.Break;
			}
			// if the object is thrown and hits anything, the state will become break
			if (collision.gameObject.tag != "Anything in the scene")
			{
				objectState = State.Break;
			}

		}
	}
	//The grounded function
	void Grounded()
	{

	}
	//When held, and the objects hits something, its state wil become dropped
	void Held()
	{
			if(hit == true)
			objectState = State.Dropped;
	}
	//the thrown state
	void Thrown()
	{
			
	}
	//if dropped, hit becomes false and the object state becomes Grounded
	void Dropped()
	{
		hit = false;
		objectState = State.Grounded;
	}
	// in this function, the object will destroy itself within a certain time
	void Break()
	{
		Destroy (gameObject,0.02f);
	}

}
