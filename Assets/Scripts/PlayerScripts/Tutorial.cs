using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public PlayerConfig pc;
	public GameObject tutorial;
	public GameObject thisTutorialObject;
	public GameObject reticule;

	public bool paused;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(paused && Input.GetKeyDown(this.pc.escape))
		{
			Unpause();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
			{
				tutorial.SetActive(true);
				reticule.SetActive(false);
				thisTutorialObject = gameObject;
				paused = true;
				Time.timeScale = 0;
			}
		 

	}

	void Unpause()
	{
			reticule.SetActive(true);
			tutorial.SetActive(false);
			Time.timeScale = 1;
			Destroy(thisTutorialObject);
	}
}
