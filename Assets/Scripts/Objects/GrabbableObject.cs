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
			Dropped,
			Break
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

			case State.Break:
			this.Break ();
			break;
		}
	}
	

	void OnCollisionEnter(Collision collision)
	{

		if(objectState == State.Held)
		{
			if(collision.gameObject.tag != "Ground" || collision.gameObject.tag !="Player")
			{
				hit = true;
			}	
		}
				
		if(objectState == State.Dropped)
		{
			if(collision.gameObject.tag == "Ground")
				{	
					objectState = State.Grounded;
				}

		}

		if (objectState == State.Thrown)
		{
			if(collision.gameObject.tag == "Light")
			{
				lightHit = collision.gameObject.GetComponentInParent<Lights>().lightObject;
				lightHit.SetActive(false);
				objectState = State.Break;
			}

			if (collision.gameObject.tag != "Anything in the scene")
			{
				objectState = State.Break;
			}

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

	void Break()
	{
		Destroy (gameObject, 0.2f);
	}

}
