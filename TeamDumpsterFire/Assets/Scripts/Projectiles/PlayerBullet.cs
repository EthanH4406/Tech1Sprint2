using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb;
	private Vector2 targetDir;
	private float speed;

	private void Awake()
	{
		rb = this.gameObject.GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		ResetProjectile();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.CompareTag("Projectile") || collision.collider.CompareTag("Player"))
		{
			Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), collision.collider, true);
		}
		else
		{
			Debug.Log("Collided with something");
			ResetProjectile();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//dmg enemy
	}

	public void Launch(Vector2 dir, float s)
	{
		this.gameObject.SetActive(true);

		targetDir = dir.normalized;
		rb.velocity = targetDir * speed;

		Debug.Log("Direction: " + targetDir.ToString() + " Speed: " + s.ToString());
	}

	public void ResetProjectile()
	{
		rb.velocity = Vector3.zero;
		this.transform.position = Vector3.zero;
		this.gameObject.SetActive(false);
	}
}
