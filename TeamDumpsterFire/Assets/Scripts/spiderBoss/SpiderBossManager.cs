using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossManager : MonoBehaviour
{
    //npc vars
    [Header("NPC Stas")]
    public float maxHP;
    private float currentHP;
    public Vector2 spawnPoint;

    public Vector2 playerPosition;
    public Vector2 currentPosition;
    public SpiderBossAnimationDriver animationDriver;
    public SpiderBossMovementDriver movementDriver;

    SpiderBossBaseState currentState;
    SpiderBossIdleState idleState = new SpiderBossIdleState();

    [Header("Debugging")]
    public bool showStateStatus;

    private void Awake()
    {
        animationDriver = this.GetComponent<SpiderBossAnimationDriver>();
    }

    void Start()
    {
        currentHP = maxHP;

        currentState = idleState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = this.gameObject.transform.position;
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnterState(this, collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnterState(this, collision);
    }

    public void SwitchState(SpiderBossBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
        {
            animationDriver.Die();
        }
    }

    public void RegenHP(float amo)
    {
        currentHP += amo;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }


}
