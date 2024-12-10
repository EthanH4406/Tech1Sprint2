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
	public Animator anim;

	public PauseMenuBehaviour pauseMenu;

	public bool enablePlayer;

	//debug
	public bool enableStateStatusReadout;
	public bool enableControlReadOut;

	public PlayerBaseState currentState;
	public PlayerReloadState reloadState = new PlayerReloadState();
	public PlayerIdleState idleState = new PlayerIdleState();
	public PlayerShootState shootState = new PlayerShootState();
	public PlayerInteractState interactState = new PlayerInteractState();
	public PlayerRepairState repairState = new PlayerRepairState();

	public InputActionReference shoot;
	public InputActionReference repair;
	public InputActionReference reload;
	public InputActionReference drop;
	public InputActionReference interact;
	public InputActionReference openMenu;

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
		enablePlayer = !pauseMenu.isPaused;

		if(enablePlayer)
		{
			currentState.UpdateState(this);
		}

	}

	private void FixedUpdate()
	{
		if(enablePlayer)
		{
			currentState.FixedUpdateState(this);
		}
			
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(enablePlayer)
		{
			currentState.OnTriggerEnterState(this, collision);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(enablePlayer)
		{
			currentState.OnCollisionEnterState(this, collision);
		}
	}

	public void SwitchState(PlayerBaseState newState)
	{
		currentState = newState;
		currentState.EnterState(this);
	}
}
