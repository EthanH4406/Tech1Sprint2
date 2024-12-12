using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
	float tmpDelay = 3;

	public override void EnterState(PlayerStateManager manager)
	{
		if(manager.enableStateStatusReadout)
		{
			Debug.Log("Entered the Idle state");
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
		////Controls
		///WASD for movement
		///Q to drop items
		///R to reload
		///E to interact
		///G to repair
		///Ignore WASD in this file, those will be handled in the movement driver

		if(manager.drop.action.ReadValue<float>() == 1)
		{
			//drop item

			if(manager.enableControlReadOut)
			{
				Debug.Log("Player Pressed the Drop Item button.");
			}
			else
			{
				//manager.inventory.DropHeldItem(manager.currentPosition);
			}
		}
		else if(manager.reload.action.ReadValue<float>() == 1)
		{
			//switch to reload state

			if(manager.enableControlReadOut)
			{
				Debug.Log("Player pressed the reload button");
			}
			else
			{
				manager.SwitchState(manager.reloadState);
			}
		}
		else if(manager.shoot.action.ReadValue<float>() == 1)
		{
			//shoot state
			
			if(manager.enableControlReadOut)
			{
				Debug.Log("Player pressed the shoot button");
			}
			else
			{
				manager.SwitchState(manager.shootState);
			}
		}
		else if(manager.interact.action.ReadValue<float>() == 1)
		{
			//interact state

			if(manager.enableControlReadOut)
			{
				Debug.Log("Player pressed the interact button");
			}
			else
			{
				manager.SwitchState(manager.interactState);
			}
		}
		else if(manager.repair.action.WasPressedThisFrame())
		{
			//repair state

			if(manager.enableControlReadOut)
			{
				Debug.Log("Player pressed the repair button");
			}
			else
			{
				manager.SwitchState(manager.repairState);
			}
		}
		else if(manager.openMenu.action.ReadValue<float>()== 1)
		{
			if(manager.enableControlReadOut)
			{
				Debug.Log("Player pressed the menu button");
			}
			else
			{
				if(manager.pauseMenu.isPaused)
				{
					manager.pauseMenu.BackToGame();
				}
				else
				{
					manager.pauseMenu.PauseGame();
				}

				
			}
		}
	}
}
