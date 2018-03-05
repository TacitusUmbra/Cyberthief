using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

public bool hit;

public State objectState;
public State defaultObjectState = State.Grounded;
public GameObject lightHit;


public enum State 
	{
		Grounded,
		Held,
		Thrown,
		Dropped
	}

void Start()
{
	objectState = defaultObjectState;
	hit = false;
}
void Update()
{
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
		}
}

void OnCollisionEnter(Collision collision)
{
	if(objectState == State.Held)
	{
		if(collision.gameObject.tag != "Ground")
		{
			hit = true;
		}	
		


	}


	if (collision.gameObject.tag == "Light")
	{
		lightHit = collision.gameObject;
		if (objectState == State.Thrown)
		{
			lightHit.SetActive(false);
		}
	}

	if(collision.gameObject.tag == "Ground")
		{
			objectState = State.Grounded;
		}

}

void Grounded()
{

}
void Held()
{
		if(hit == true)
		objectState = State.Dropped;
}
void Thrown()
{
	
}
void Dropped()
{
	hit = false;
	objectState = State.Grounded;
}

}
