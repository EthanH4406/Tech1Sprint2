using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState 
{
	public abstract void EnterState(PlayerStateManager manager);
	public abstract void ExitState(PlayerStateManager manager);
	public abstract void UpdateState(PlayerStateManager manager);
	public abstract void FixedUpdateState(PlayerStateManager manager);
	public abstract void OnTriggerEnterState(PlayerStateManager manager, Collider2D collision);
	public abstract void OnCollisionEnterState(PlayerStateManager manager, Collision2D collision);
}
