using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

public bool hit;


	 void OnCollisionEnter(Collision collision)
    {
			if(collision.gameObject.tag != "Ground")
			hit = true;

			if(collision.gameObject.tag == "Ground")
			hit = false;
	}
}
