using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootState : PlayerBaseState
{
	private GunBehaviour gun;
	private float timer;

	public override void EnterState(PlayerStateManager manager)
	{
		if(manager.enableStateStatusReadout)
		{
			Debug.Log("Entered the Shoot State");
		}

		timer = Time.time;

		/*
		if(manager.inventory.GetHeldItem().tag.Equals("gun"))
		{
			gun = manager.inventory.GetHeldItem().gameObject.GetComponent<GunBehaviour>();

			GameObject bullet = manager.gameManager.objPooler.SpawnFromPool("player_bullet", manager.currentPosition, Quaternion.identity);
			//shoot bullet
		}
		*/

		
		if(manager.ammoCounter.ammoCount > 0)
		{
			manager.anim.SetBool("shooting", true);
			manager.ammoCounter.ShootBullet();
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
