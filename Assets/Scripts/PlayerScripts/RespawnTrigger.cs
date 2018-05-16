using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour {

	public Vector3 newRespawnZone;
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<Health>().respawnZone = newRespawnZone;
			Destroy(this);
		}

	}
}
