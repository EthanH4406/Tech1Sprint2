using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerBaseState
{
	public override void EnterState(PlayerStateManager manager)
	{
		if(manager.enableStateStatusReadout)
		{
			Debug.Log("Entered the interact state");
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
		if(collision.CompareTag("trashHeap"))
		{
			TrashFishingBehaviour trash = collision.gameObject.GetComponent<TrashFishingBehaviour>();
			trash.Interact();
			manager.SwitchState(manager.idleState);
		}
	}

	public override void UpdateState(PlayerStateManager manager)
	{
		
	}
}
