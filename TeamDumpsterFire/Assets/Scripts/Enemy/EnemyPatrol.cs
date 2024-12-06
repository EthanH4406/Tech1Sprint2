using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPatrol : MonoBehaviour
{
    public AIDestinationSetter targetSetter;
    GameObject[] waypoints;
    public Transform enemyPos;
    public Transform playerPos;
    public float threshold;
    public float playerThreshold;
    public float destOffset;
    bool allowPathing;
    int destSelected;
    float timer;
    public float timerCooldown;

    private Vector2 posOfWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        allowPathing = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        SetDestination();
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDist = Vector2.Distance(enemyPos.position, playerPos.position);
        if (playerDist > playerThreshold)
        {
            allowPathing = false;
        }
        else
        {
            allowPathing = true;
        }

        
        if(Vector2.Distance(posOfWaypoint, enemyPos.position) <= destOffset && Time.time >= timer + timerCooldown && allowPathing)
        {
            SetDestination();
        }
        
    }

    void SetDestination()
    {
        if (waypoints == null)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Basic Enemy Waypoint");
        }

        destSelected = Random.Range(0, waypoints.Length);

        posOfWaypoint = waypoints[destSelected].transform.position;

        float dist = Vector2.Distance(enemyPos.position, waypoints[destSelected].transform.position);
        while (dist > threshold)
        {
            destSelected = Random.Range(0, waypoints.Length);
            posOfWaypoint = waypoints[destSelected].transform.position;
            dist = Vector2.Distance(enemyPos.position, waypoints[destSelected].transform.position);
        }

        targetSetter.target = waypoints[destSelected].transform;
    }
}
