
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public GameObject nightstick;
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
			EquipWeapon,
			HoldWeapon,
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
		case State.EquipWeapon:
			this.EquipWeapon ();
			break;
		case State.HoldWeapon:
			this.HoldWeapon ();
			break;
		case State.CarryBody:
			this.CarryBody ();
			break;
			
		}
	}
	void EquipDevice()
	{

		nightstick.SetActive(false);
		device.SetActive(true);
		equipState = State.HoldDevice;

	}

	void HoldDevice()
	{

		if(Input.GetKey(pc.nightstickKey))
		equipState = State.EquipWeapon;

		if(interact.bodyHeld)
		{
		previousEquipState = State.HoldDevice;
		equipState = State.CarryBody;
		}
	}

	void EquipWeapon()
	{
		device.SetActive(false);
		nightstick.SetActive(true);
		equipState = State.HoldWeapon;
	}
	void HoldWeapon()
	{
		if(Input.GetKey(pc.deviceKey))
		equipState = State.EquipDevice;

		if(interact.bodyHeld)
		{
		previousEquipState = State.HoldWeapon;
		equipState = State.CarryBody;
		}
	}

	void CarryBody()
	{
	device.SetActive(false);
	nightstick.SetActive(false);
		if(!interact.bodyHeld)
		{
		if(previousEquipState == State.HoldDevice)
		  equipState = State.EquipDevice;
		if(previousEquipState == State.HoldWeapon)
		  equipState = State.EquipWeapon;


		}


	}

}
