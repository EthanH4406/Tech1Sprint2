using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFOV : MonoBehaviour
{
    public EnemyPatrol enemyPatrol;
    public CircleCollider2D viewRadius;
    public LayerMask obstacleMask;

    // Start is called before the first frame update
    void Start()
    {
        enemyPatrol.targetFound = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(viewRadius.IsTouching(collision) && collision.gameObject.CompareTag("Player"))
        {
            Transform target = collision.gameObject.transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float dstToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
            {
                enemyPatrol.targetFound = true;
            }
        }
    }
}
