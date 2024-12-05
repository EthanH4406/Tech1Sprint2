using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossIdleState : SpiderBossBaseState
{
    private float timer;
    private float stateSwapCooldown = 1f;

    public override void EnterState(SpiderBossManager manager)
    {
        if(manager.showStateStatus)
        {
            Debug.Log("Entered the Idle State");
        }

        timer = Time.time;
        manager.movementDriver.MoveTo(manager.spawnPoint, 3f);
    }

    public override void ExitState(SpiderBossManager manager)
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdateState(SpiderBossManager manager)
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnterState(SpiderBossManager manager, Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnterState(SpiderBossManager manager, Collider2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(SpiderBossManager manager)
    {
        if(timer >= Time.time + stateSwapCooldown)
        {
            int rng = Random.Range(0, 6);       //random number between 0 (inclusive) and number of states + 1

            switch(rng)
            {
                case 0:
                    break;

                case 1:
                    break;
            }
        }
    }
}
