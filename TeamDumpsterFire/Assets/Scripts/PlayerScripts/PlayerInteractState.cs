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

		//check a circle radius for any gameobjects with the tag trashHeap
		//get that game object
		//call the spawn function

		float interactRadius = 1f;

		RaycastHit2D[] foundColliders = Physics2D.CircleCastAll(manager.currentPosition, interactRadius, Vector2.up);

		for(int i=0; i<foundColliders.Length; i++)
		{
			if (foundColliders[i].collider.CompareTag("trashHeap"))
			{
				TrashFishingBehaviour fishingSpawner = foundColliders[i].collider.gameObject.GetComponent<TrashFishingBehaviour>();
				fishingSpawner.SpawnItem();
				break;

			}
		}

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
