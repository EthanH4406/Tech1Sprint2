using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObjectPoolerSystem objPooler;


	private GameObject[] npcs;
	private GameObject player;

	private void Awake()
	{
		objPooler = this.gameObject.GetComponent<ObjectPoolerSystem>();

	}


}
