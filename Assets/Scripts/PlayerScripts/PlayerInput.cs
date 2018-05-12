using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	//Creating a GameObject for the PlayerConfig to sit in
	public PlayerConfig pc;
	//Creating a public Vector3 direction
	public Vector3 direction;
	//Creating a public Vector3 rotation
	public Vector3 rotation;

	void Update () {
		// This will check the direction each frame 
		this.SetDirection ();
		//This will check the rotation each frame
		this.SetRotation ();
	}

	//When the chracter moves in a particular direction, use the Keycodes from the PlayerConfig to move the player in said direction relative to the player inside of the scene
	void SetDirection () {
		//Direction is Vector3.zero, or Vector(0,0,0)
		this.direction = Vector3.zero;
		//Move forward if I keep my finger on the key set inside PlayerConfig for forward (w)
		if (Input.GetKey (this.pc.forward)) {
			this.direction += Vector3.forward;
			//Move backwards if I keep my finger on the key set inside PlayerConfig for back (s)
		} else if (Input.GetKey (this.pc.backwards)) {
			this.direction += Vector3.back;
		}
		//Move left if I keep my finger on the key set inside PlayerConfig for left (a)
		if (Input.GetKey (this.pc.left)) {
			this.direction += Vector3.left;
			//Move Right if I keep my finger on the key set inside PlayerConfig for right (d)
		} else if (Input.GetKey (this.pc.right)) {
			this.direction += Vector3.right;
		}  

			//Normalizing the direction to maintain a magnitude of 1
			this.direction = this.direction.normalized;
		}

	//Verifies the rotation of the mouse 
	void SetRotation () {
		//When the player moves the mouse in the horizontal position, they are affecting the yaw. 


			float yaw = Input.GetAxis ("Mouse X") * this.pc.mouseSensitivity;
			//When the player moves the mouse in the vertical position, they are affecting the pitch. 
		

		float pitch = Input.GetAxis("Mouse Y") * this.pc.mouseSensitivity;
		//Offers the option to invert the pitch, or in other words, up is down and down is up when you move the mouse along the 		vertical axis
		if (this.pc.invertY)
		{
			pitch *= -1;
		}
		
		this.rotation = new Vector3(yaw, pitch, 0f);

	}
}