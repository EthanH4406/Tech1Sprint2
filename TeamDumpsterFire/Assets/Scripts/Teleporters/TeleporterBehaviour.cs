using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBehaviour : MonoBehaviour
{
    private GameObject exit;

    private GameObject player;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		exit = GameObject.FindGameObjectWithTag("dEntrance");
	}

	private void Update()
	{
		if(exit == null)
		{
			exit = GameObject.FindGameObjectWithTag("dEntrance");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			player.transform.position = exit.transform.position;
		}
	}
}