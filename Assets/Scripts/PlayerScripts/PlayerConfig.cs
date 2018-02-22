﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//These are the controls setup for the direction which the player will go and the actions which they may take
public class PlayerConfig : MonoBehaviour {
	public KeyCode left;
	public KeyCode right;
	public KeyCode forward;
	public KeyCode backwards;
	public KeyCode jump;
	public KeyCode run;
	public KeyCode crouch;
	public KeyCode interact;
	public KeyCode alternativeInteract;
	public bool invertY;
	public float mouseSensitivity;

}