using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObjectPoolerSystem objPooler;


	private GameObject[] npcs;
	private GameObject player;

	public GameObject[] waypoints;

	private void Awake()
	{
		objPooler = this.gameObject.GetComponent<ObjectPoolerSystem>();

	}

	private void Start()
	{
		StartCoroutine(GetWaypoints());
	}

	IEnumerator GetWaypoints()
	{
		yield return new WaitForSeconds(1.0f);

		waypoints = GameObject.FindGameObjectsWithTag("Basic Enemy Waypoint");
	}


}
