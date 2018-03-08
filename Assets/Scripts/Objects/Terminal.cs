using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour {

	public GameObject door;
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

		Debug.Log(percentageHacked);
			if(percentageHacked >= 100f)
			{
				door.gameObject.GetComponent<Door> ().locked = false;
				percentageHacked = 100;
			}

			if(autohack && percentageHacked <= 100)
			{
				percentageHacked += 1 * Time.deltaTime;
			}

	}
}
