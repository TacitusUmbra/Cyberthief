
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public GameObject comlink;
	public GameObject device;
	public Interact interact;

	public State equipState;
	public State defaultEquipState = State.HoldDevice;
	public State previousEquipState;
	public PlayerConfig pc;

	public float numberOfDecoders;
	public bool keycardLevelOne;
	public bool keycardLevelTwo;



	public enum State 
		{
			EquipDevice,
			HoldDevice,
			HoldComDevice,
			EquipComDevice,
			CarryBody
		}
	// Use this for initialization
	void Start () 
	{
		equipState = defaultEquipState;
		device.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (this.equipState)
		{
	
		case State.EquipDevice:
			this.EquipDevice ();
			break;
		case State.HoldDevice:
			this.HoldDevice ();
			break;
		case State.EquipComDevice:
			this.EquipComDevice ();
			break;
		case State.HoldComDevice:
			this.HoldComDevice ();
			break;
		case State.CarryBody:
			this.CarryBody ();
			break;
			
		}
	}
	void EquipDevice()
	{

		comlink.SetActive(false);
		device.SetActive(true);
		equipState = State.HoldDevice;

	}

	void HoldDevice()
	{

		if(Input.GetKey(pc.comlinkKey))
		equipState = State.EquipComDevice;

		if(interact.bodyHeld)
		{
		previousEquipState = State.HoldDevice;
		equipState = State.CarryBody;
		}
	}

	void EquipComDevice()
	{
		device.SetActive(false);
		comlink.SetActive(true);
		equipState = State.HoldComDevice;
	}
	void HoldComDevice()
	{
		if(Input.GetKey(pc.deviceKey))
		equipState = State.EquipDevice;

		if(interact.bodyHeld)
		{
		previousEquipState = State.HoldComDevice;
		equipState = State.CarryBody;
		}
	}

	void CarryBody()
	{
	device.SetActive(false);
	comlink.SetActive(false);
		if(!interact.bodyHeld)
		{
		if(previousEquipState == State.HoldDevice)
		  equipState = State.EquipDevice;
		if(previousEquipState == State.HoldComDevice)
		  equipState = State.EquipComDevice;


		}


	}

}
