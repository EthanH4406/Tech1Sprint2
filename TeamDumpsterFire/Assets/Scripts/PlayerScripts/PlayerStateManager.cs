using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{


	[Header("Fill These")]
	public PauseMenuBehaviour pauseMenu;
	public Animator anim;
	public ParticleSystem repairParticles;

	[Header("Player Controls")]
	public InputActionReference shoot;
	public InputActionReference repair;
	public InputActionReference reload;
	public InputActionReference drop;
	public InputActionReference interact;
	public InputActionReference openMenu;

	//debug
	[Header("Debug")]
	public bool enableStateStatusReadout;
	public bool enableControlReadOut;
	public bool buffreadout;

	[Header("Don't Touch")]
	public PlayerBaseState currentState;
	public PlayerReloadState reloadState = new PlayerReloadState();
	public PlayerIdleState idleState = new PlayerIdleState();
	public PlayerShootState shootState = new PlayerShootState();
	public PlayerInteractState interactState = new PlayerInteractState();
	public PlayerRepairState repairState = new PlayerRepairState();
	public PlayerAmmoCounter ammoCounter;
	public PlayerHealthBar healthBar;
	public GameManager gameManager;
	public bool enablePlayer;
	public Vector2 currentPosition;
	public float gunTimer;


	private void Awake()
	{
		//animationDriver = this.GetComponent<SpiderBossAnimationDriver>();

		
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

		//pickups
		if(collision.CompareTag("pickHP"))
		{
			//hp pickup
			healthBar.Heal();
			collision.gameObject.GetComponent<GenericBuff>().ResetBuff();

			if(buffreadout)
			{
				Debug.Log("Picked up HP pickup.");
			}
		}
		else if(collision.CompareTag("pickAmmo"))
		{
			//ammo pick
			ammoCounter.IncreaseAmmoCount();
			collision.gameObject.GetComponent<GenericBuff>().ResetBuff();

			if (buffreadout)
			{
				Debug.Log("Picked up Ammo pickup.");
			}
		}
		else if(collision.CompareTag("pickDmg"))
		{
			//dmg increase
			ammoCounter.IncreaseDamage();
			collision.gameObject.GetComponent<GenericBuff>().ResetBuff();

			if (buffreadout)
			{
				Debug.Log("Picked up Dmg pickup.");
			}
		}
		else if(collision.CompareTag("pickSpeed"))
		{
			//speed inc
			ammoCounter.IncreaseFirerate();
			collision.gameObject.GetComponent<GenericBuff>().ResetBuff();

			if (buffreadout)
			{
				Debug.Log("Picked up Speed pickup.");
			}
		}
		else if(collision.CompareTag("pickHurt"))
		{
			healthBar.TakeDamage();
			collision.gameObject.GetComponent<GenericBuff>().ResetBuff();

			if(buffreadout)
			{
				Debug.Log("Picked up Hurtful pickup");
			}

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
