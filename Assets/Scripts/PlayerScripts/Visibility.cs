using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Visibility : MonoBehaviour {

	public Player pl;
	public string visibilityString;
	public float newVisibility;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
		newVisibility = pl.gameObject.GetComponent<Player>().visibility;
		newVisibility = Mathf.RoundToInt(newVisibility);
		visibilityString = newVisibility.ToString();
		gameObject.GetComponent<Text>().text = visibilityString;
	}
}
