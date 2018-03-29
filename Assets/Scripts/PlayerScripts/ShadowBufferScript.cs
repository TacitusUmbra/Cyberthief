using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBufferScript : MonoBehaviour {

	public RenderTexture shadowBuffer;
	Texture2D tex2d;

	public float percentageHidden;
	public float relativeLuminance;

	void Start(){
		tex2d = new Texture2D (shadowBuffer.width, shadowBuffer.height, TextureFormat.RGB24, false);
	}

	// Update is called once per frame
	void Update () {

		tex2d = new Texture2D (shadowBuffer.width, shadowBuffer.height, TextureFormat.RGB24, false);

		RenderTexture.active = shadowBuffer;
		tex2d.ReadPixels (new Rect (0, 0, shadowBuffer.width, shadowBuffer.height), 0, 0);
		tex2d.Apply ();

		Color shadowBufferColor = tex2d.GetPixel (0, 0);
		relativeLuminance = shadowBufferColor.r * 0.2126f + shadowBufferColor.g * 0.7152f + shadowBufferColor.b * 0.0722f;

		percentageHidden = relativeLuminance * 100;
		Debug.Log (percentageHidden);

	}
}
