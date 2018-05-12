using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
	//Player config goes here
	public PlayerConfig pc;
	// Tutorial gameobject goes here
	public GameObject tutorial;
	// collider goes here
	public GameObject thisTutorialObject;
	//reticule goes here
	public GameObject reticule;
	//checking if the game is paused
	public bool paused;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{	//if the game is paused and the player presses escape, unpause
		if(paused && Input.GetKeyDown(this.pc.escape))
		{
			Unpause();
		}
	}
	// As the player enters the collider, pause the game and show the tutorial
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
	//If unpaused, destroy the collider and unpause the game
	void Unpause()
	{
			reticule.SetActive(true);
			tutorial.SetActive(false);
			Time.timeScale = 1;
			Destroy(thisTutorialObject);
	}
}
