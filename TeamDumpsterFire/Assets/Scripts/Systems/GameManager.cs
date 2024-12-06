using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObjectPoolerSystem objPooler;

	private void Awake()
	{
		objPooler = this.gameObject.GetComponent<ObjectPoolerSystem>();
	}
}
