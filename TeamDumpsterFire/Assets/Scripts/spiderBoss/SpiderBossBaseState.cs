using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpiderBossBaseState
{
    public abstract void EnterState(SpiderBossManager manager);
    public abstract void ExitState(SpiderBossManager manager);
    public abstract void UpdateState(SpiderBossManager manager);
    public abstract void FixedUpdateState(SpiderBossManager manager);
    public abstract void OnTriggerEnterState(SpiderBossManager manager, Collider2D collision);
    public abstract void OnCollisionEnterState(SpiderBossManager manager, Collision2D collision);

}
