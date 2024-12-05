using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
	private Vector3 initPos;

	public float speed = 3f;
	public float height = 0.25f;

	public string type;
	public int ammount;

	private void Start()
	{
		initPos = transform.position;
	}

	private void FixedUpdate()
	{
		float newY = Mathf.Sin(Time.time * speed) * height;
		transform.position = new Vector3(initPos.x, newY + initPos.y, 0);
	}
}
