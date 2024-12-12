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

    public AIPath aiPath;
    public bool targetFound = false;

    private Vector2 posOfWaypoint;

    public PlayerHealthBar playerHealth;
    public float hitDelay;
    private GameManager gameManager;

	private void Awake()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	// Start is called before the first frame update
	void Start()
    {
        allowPathing = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthBar>();
        //aiPath = GetComponent<AIPath>();
        SetDestination();
        timer = Time.time;
        aiPath.canMove = false;
        StartCoroutine("HitDelay", 0.5f);
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

        
        if(Vector2.Distance(posOfWaypoint, enemyPos.position) <= destOffset && Time.time >= timer + timerCooldown && allowPathing && !targetFound && gameManager.waypoints.Length > 0)
        {
            SetDestination();
        }
        if (targetFound && targetSetter.target != playerPos)
        {
            targetSetter.target = playerPos;
        }
    }

    void SetDestination()
    {
        waypoints = gameManager.waypoints;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<BoxCollider2D>().IsTouching(collision) && collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage();
            aiPath.canMove = false;
            StartCoroutine("HitDelay", hitDelay);
        }
    }

    IEnumerator HitDelay(float hitDelay)
    {
        yield return new WaitForSeconds(hitDelay);
        aiPath.canMove = true;
    }
}
