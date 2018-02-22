using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHearingSensor : MonoBehaviour {

	public bool isHearingPlayerRun;
	public Vector3 hsTarget;

	// Use this for initialization
	void Start () {
		isHearingPlayerRun = false;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().currentState == Player.State.Run )
		{
			isHearingPlayerRun = true;
			hsTarget = other.transform.position;
		}

		//To fix afterwards
		/*else if (other.gameObject.tag == "Breakable" && other.gameObject.GetComponent<Throwing>().crashed == true)
		{

			isHearingPlayerRun = true;
			hsTarget = other.transform.position;

		}*/
		
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			isHearingPlayerRun = false;


	}



		


}
	
	

	

