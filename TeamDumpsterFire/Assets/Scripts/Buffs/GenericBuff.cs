using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBuff : MonoBehaviour
{
	private Vector3 initPos;

	public float speed = 3f;
	public float height = 0.25f;

	

	private void Start()
	{
		initPos = transform.position;
	}

	private void FixedUpdate()
	{
		float newY = Mathf.Sin(Time.time * speed) * height;
		transform.position = new Vector3(initPos.x, newY + initPos.y, 0);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(!collision.CompareTag("Player"))
		{
			return;
		}
		else
		{
			//ResetBuff();
		}
	}

	public void ResetBuff()
	{
		transform.position = Vector3.zero;
		this.gameObject.SetActive(false);
	}

	public void ActivateBuff(Vector3 pos)
	{
		this.gameObject.SetActive(true);
		this.transform.position = pos;
	}

}
