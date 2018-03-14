using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainBox : MonoBehaviour {
	public float attackTimer;
	public GameObject enemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (attackTimer < 2.2)
			attackTimer += 1 * Time.deltaTime;
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player" && enemy.GetComponent<PatrolAI>().aiCurrentState == PatrolAI.State.Hostile)
			if(attackTimer > 2.1)
			{
				other.GetComponent<Health>().health -= 1;
				attackTimer = 0;
			}

	}
}
