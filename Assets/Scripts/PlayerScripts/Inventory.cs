
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	//comlink goes here
	public GameObject comlink;
	//device goes here
	public GameObject device;
	//interact goes here
	public Interact interact;
	//Equip State
	public State equipState;
	//Default equipt state is holdDevice
	public State defaultEquipState = State.HoldDevice;
	//previous equip state
	public State previousEquipState;
	//player config goes here
	public PlayerConfig pc;
	// number of decoders the player has
	public float numberOfDecoders;
	// if the player has a keycard level one
	public bool keycardLevelOne;
	//if the player has a keycard level two
	public bool keycardLevelTwo;


	// inventory states
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
	{//equipstate is the defaultequipstate
		equipState = defaultEquipState;
		//the device is active
		device.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () 
	{
		//The states 
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
	//The player equips the device and the state becomes HoldDevice. The comlink is inactive
	void EquipDevice()
	{

		comlink.SetActive(false);
		device.SetActive(true);
		equipState = State.HoldDevice;

	}
	//the player is holding the device. If they hold a body, the previous state will be HoldDevice and the new state is CarryBody
	//if they press a key, they will equip the comlink
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
	//Equping the comlink
	void EquipComDevice()
	{
		device.SetActive(false);
		comlink.SetActive(true);
		equipState = State.HoldComDevice;
	}
	//Holding the comdevice.If they hold a body, the previous state will be HoldDevice and the new state is CarryBody
	//if they press a key, they will equip the device
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
	// The device and comlink are inactive. If they no longer hold a body, the previous equip state is the equip state
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
