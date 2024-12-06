using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerSystem : MonoBehaviour
{
	public Dictionary<string, Queue<GameObject>> poolDict;
	public List<Pool> pools;

	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	private void Start()
	{
		poolDict = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDict.Add(pool.tag, objectPool);
		}
	}

	public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rotation)
	{
		if (!poolDict.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " does not exist.");
			return null;
		}

		GameObject objToSpawn = poolDict[tag].Dequeue();
		objToSpawn.SetActive(true);
		objToSpawn.transform.position = pos;
		objToSpawn.transform.rotation = rotation;

		poolDict[tag].Enqueue(objToSpawn);
		return objToSpawn;
	}
}
