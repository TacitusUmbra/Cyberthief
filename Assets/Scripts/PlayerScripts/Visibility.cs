using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Visibility : MonoBehaviour {
	//Player script goes here
	public Player pl;
	// the string of the visibility
	public string visibilityString;
	//the float of visibility
	public float newVisibility;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
		// newVisibility is the player's visibility
		newVisibility = pl.gameObject.GetComponent<Player>().visibility;
		//round the visibility to the Int
		newVisibility = Mathf.RoundToInt(newVisibility);
		// turn the newVisibility to a string
		visibilityString = newVisibility.ToString();
		//the text of will display the visibility as a string
		gameObject.GetComponent<Text>().text = visibilityString;
	}
}
