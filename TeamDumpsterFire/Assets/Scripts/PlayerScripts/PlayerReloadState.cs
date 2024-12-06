using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadState : PlayerBaseState
{
	public override void EnterState(PlayerStateManager manager)
	{
		if(manager.enableStateStatusReadout)
		{
			Debug.Log("Entered the Reload State");
		}

		//---------------------------
		PlayerItem heldItem = manager.inventory.GetHeldItem();

		if(heldItem.type.Equals("gun"))
		{
			GunBehaviour gun = heldItem.gameObject.GetComponent<GunBehaviour>();

			if (gun.currentAmmoCount < gun.maxAmmoCount)
			{
				//block controls
				//play reload animation
				//reset ammo
				//subtract ammo from total ammo

				gun.Reload();
			}
		}
		else
		{
			//the player isn't holding a gun, nothing to do
			manager.SwitchState(manager.idleState);
		}

	}

	public override void ExitState(PlayerStateManager manager)
	{
		
	}

	public override void FixedUpdateState(PlayerStateManager manager)
	{
		
	}

	public override void OnCollisionEnterState(PlayerStateManager manager, Collision2D collision)
	{
		
	}

	public override void OnTriggerEnterState(PlayerStateManager manager, Collider2D collision)
	{
		
	}

	public override void UpdateState(PlayerStateManager manager)
	{
		
	}
}
