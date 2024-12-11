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
		if(manager.ammoCounter.ammoCount < manager.ammoCounter.magCount)
		{
			manager.ammoCounter.Reload();
		}

		//manager.ammoCounter.Reload();

		manager.SwitchState(manager.idleState);
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
