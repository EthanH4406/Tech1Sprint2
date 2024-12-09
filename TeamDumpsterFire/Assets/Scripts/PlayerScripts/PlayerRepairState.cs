using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairState : PlayerBaseState
{
	public override void EnterState(PlayerStateManager manager)
	{
		if(manager.enableStateStatusReadout)
		{
			Debug.Log("Entered the repair state");
		}

		manager.ammoCounter.Repair();
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
