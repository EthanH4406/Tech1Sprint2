using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
	public PlayerInventory inventory;
	public PlayerAmmoCounter ammoCounter;
	public PlayerHealthBar healthBar;
	public GameManager gameManager;

	//debug
	public bool enableStateStatusReadout;
	public bool enableControlReadOut;

	public PlayerBaseState currentState;
	public PlayerReloadState reloadState = new PlayerReloadState();
	public PlayerIdleState idleState = new PlayerIdleState();
	public PlayerShootState shootState = new PlayerShootState();
	public PlayerInteractState interactState = new PlayerInteractState();

	public InputActionReference shoot;
	public InputActionReference repair;
	public InputActionReference reload;
	public InputActionReference drop;
	public InputActionReference interact;

	public Vector2 currentPosition;
	public float gunTimer;

	private void Awake()
	{
		//animationDriver = this.GetComponent<SpiderBossAnimationDriver>();

		inventory = this.gameObject.GetComponent<PlayerInventory>();
		ammoCounter = this.gameObject.GetComponent<PlayerAmmoCounter>();
		healthBar = this.gameObject.GetComponent<PlayerHealthBar>();
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void Start()
	{
		gunTimer = Time.time;

		currentState = idleState;
		currentState.EnterState(this);
	}

	// Update is called once per frame
	void Update()
	{
		currentPosition = this.gameObject.transform.position;



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

	public void SwitchState(PlayerBaseState newState)
	{
		currentState = newState;
		currentState.EnterState(this);
	}
}
