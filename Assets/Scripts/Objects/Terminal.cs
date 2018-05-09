using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour {

	public GameObject door;
	public GameObject vent;
	public float percentageHacked;
	public bool hacked;
	public bool autohack;

	// Use this for initialization
	void Start () 
	{
		hacked = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{

			if(percentageHacked >= 100f)
			{

				if(door)
				{
				door.gameObject.GetComponent<Door> ().locked = false;
				percentageHacked = 100;
				}

				if(vent)
				{
					
				}


			}

			if(autohack && percentageHacked <= 100)
			{
				percentageHacked += 1 * Time.deltaTime;
			}

	}
}
