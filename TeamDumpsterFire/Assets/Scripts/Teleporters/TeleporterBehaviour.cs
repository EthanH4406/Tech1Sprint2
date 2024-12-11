using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBehaviour : MonoBehaviour
{
    private Vector3 entrancePos;
    private Vector3 exitPos;

	public GameObject entranceObj;
	public GameObject exitObj;

	private GameObject player;


	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Start()
	{
		entrancePos = entranceObj.transform.position;
		exitPos = exitObj.transform.position;
	}

	private void Update()
	{
		if(entranceObj.GetComponent<CircleCollider2D>().bounds.Contains(player.transform.position))
		{
			player.transform.position = exitPos;
		}
	}

	public void SetTeleporter(Vector3 a, Vector3 b)
	{
		entranceObj.transform.position = a;
		exitObj.transform.position = b;
	}
}
