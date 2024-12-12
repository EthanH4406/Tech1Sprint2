using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	public string type;
	public float speed = 4.5f;

	private GameManager gameManager;
	private bool enable;
	private Vector2 targetDirection;
	private float projectileLifeTime = 5f;
	private float timer;
	private Rigidbody2D rb;
	private Vector3 v;

	private void Awake()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		rb = this.gameObject.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		//transform.position += transform.right * Time.deltaTime * speed;
		v = rb.velocity;

		float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	}

	private void FixedUpdate()
	{
		if (enable)
		{
			if (Time.time > projectileLifeTime + timer)
			{
				ResetProjectile();
			}
			else
			{
				//rb.MovePosition(rb.position + targetDirection * speed * Time.deltaTime);
				//rb.velocity = targetDirection * speed;
			}


		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Destroy(gameObject);

		if (collision.collider.CompareTag("Projectile") || collision.collider.CompareTag("Player"))
		{
			Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), collision.collider, true);
			return;
		}
		else
		{
			ResetProjectile() ;
		}

		if (Time.time > projectileLifeTime + timer)
		{
			ResetProjectile();
		}
		

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
        {
			ResetProjectile();
			collision.GetComponent<EnemyPatrol>().EnemyTakeDamage(1);		
		}
	}

	public void Launch(Vector2 direction, float _speed)
	{
		this.gameObject.SetActive(true);
		enable = true;
		targetDirection = direction.normalized;
		speed = _speed;
		timer = Time.time;
		//projectileLifeTime = lifetime;

		rb.velocity = (Vector3)targetDirection * speed;

	}

	public void ResetProjectile()
	{
		this.transform.position = Vector3.zero;
		this.gameObject.SetActive(false);
		enable = false;
	}

}
