using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour {

	//The door associated with the terminal
	public GameObject door;
	//The percentage hacked of the terminal
	public float percentageHacked;
	//The bool determining whether the terminal was hacked or not
	public bool hacked;
	//The bool determining whether the terminal was autohacked or not
	public bool autohack;

	public float autohackRate;

	// Use this for initialization
	void Start () 
	{
		//Hacked is false
		hacked = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{	
			//If the percentage hacked is over 100, the door will be unlocked and the percentage hacked is set to 100
			if(percentageHacked >= 100f)
			{
				door.gameObject.GetComponent<Door> ().locked = false;
				percentageHacked = 100;
				
			}
			//If autohacked and the percentage hacked is less than 100, the percentage hacked will increase over time
			if(autohack && percentageHacked <= 100)
			{
				percentageHacked += autohackRate * Time.deltaTime;
			}

	}
}
