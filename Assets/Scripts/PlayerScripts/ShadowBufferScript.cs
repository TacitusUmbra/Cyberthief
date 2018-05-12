using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBufferScript : MonoBehaviour {
	//Shadowbugger render texture
	public RenderTexture shadowBuffer;
	// The texture
	Texture2D tex2d;
	//Percentage hidden
	public float percentageHidden;
	//Relative luminance
	public float relativeLuminance;

	void Start(){
		//The shadowbuffer width, height, and formath in RGB becomes the tex2d
		tex2d = new Texture2D (shadowBuffer.width, shadowBuffer.height, TextureFormat.RGB24, false);
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Getting the values from the shadowbuffer
		tex2d = new Texture2D (shadowBuffer.width, shadowBuffer.height, TextureFormat.RGB24, false);
		RenderTexture.active = shadowBuffer;
		tex2d.ReadPixels (new Rect (0, 0, shadowBuffer.width, shadowBuffer.height), 0, 0);	
		tex2d.Apply ();
		// shadowbuffer color
		Color shadowBufferColor = tex2d.GetPixel (0, 0);
		// relative luminance is the shadowbuffer rbg multiplied by some values
		relativeLuminance = shadowBufferColor.r * 0.2126f + shadowBufferColor.g * 0.7152f + shadowBufferColor.b * 0.0722f;
		//The percentage hidden is the relative luminance multiplied by 100
		percentageHidden = relativeLuminance * 100;

	}
}
