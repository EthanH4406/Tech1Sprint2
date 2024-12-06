using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class CreateDungeon : MonoBehaviour
{
    private DungeonGenerator generator;

    [Header("General Placement Settings")]
    [Tooltip("The percentage of points that get accepted as valid.")]
	[SerializeField]
	[Range(0, 100)]
	private int density = 10;
    [Tooltip("The radius in which points cannot generate relative to each other. Optimal distance is 1.")]
	[SerializeField]
	private float antiSpawnRadius = 1;
    [Tooltip("The number of attempts at spawning a point at a relative position.")]
	[SerializeField]
	private int numberOfSamplesBeforeRejection = 30;
    [Tooltip("The minimum distance between the start and end position.")]
    [SerializeField]
    private float dstFromStartToEnd;

    [Header("Assets")]
    [Tooltip("This is the asset the player will get teleported to when they start the level.")]
	[SerializeField]
    private GameObject startPosAsset;
    [Tooltip("This is the asset that will teleport the player out of the dungeon and into the boss arena.")]
	[SerializeField]
    private GameObject dungeonExitAsset;
    [Tooltip("A list of assets that will be randomly spread around the map.")]
    [SerializeField]
    private List<GameObject> listOfAssetsToSpread = new List<GameObject>();

    [Header("Debug")]
    [Tooltip("Will show all of the spawn points generated.")]
    [SerializeField]
    private bool showPoints;
    [Tooltip("Enable distance read outs between the start and end positions.")]
    [SerializeField]
    private bool printDst;
    [Tooltip("This is a list of all the spawned assets. DO NOT TOUCH.")]
    [SerializeField]
    private List<GameObject> spawnedObjects = new List<GameObject>();

	private HashSet<Vector2Int> actualPositions = new HashSet<Vector2Int>();
    private AstarPath pathfinding;

    private float tempTimer;

	private void Awake()
	{
		generator = GetComponent<DungeonGenerator>();
        pathfinding = GameObject.FindGameObjectWithTag("Pathfinding").GetComponent<AstarPath>();
	}

	void Start()
    {
        tempTimer = Time.time;
        bool status = CreateJunkyard();

        Debug.Log("finish status: " + status.ToString());
    }

    // Update is called once per frame
    

    public bool CreateJunkyard()
    {
        bool mapGenSuccess = generator.GenerateDungeonMap();
        bool populatedMap = false;

        Debug.Log("MapGenSuccess: " + mapGenSuccess.ToString());

		if (mapGenSuccess)
        {
            populatedMap = PopulateMapWithAssets();
        }

        Debug.Log("Populated Map: " + populatedMap.ToString());

        if(populatedMap)
        {


			if(pathfinding != null)
            {
				AstarPath.active.UpdateGraphs(generator.wallsTilemap.gameObject.GetComponent<TilemapCollider2D>().bounds, 0.5f);
				AstarPath.active.Scan();
			}


			return true;
        }

        return false;
    }

    private void ClearAssets()
    {
        for(int i=0; i< spawnedObjects.Count; i++)
        {
            Destroy(spawnedObjects[i]);

        }

        spawnedObjects.Clear();
    }

	private bool PopulateMapWithAssets()
	{
		List<BoundsInt> generatedRooms = generator.generatedRooms;
	    HashSet<Vector2Int> generatedCorridors = generator.generatedCorridors;
	    HashSet<Vector2Int> generatedFloorTiles = generator.generatedFloorTiles;

        Vector2 dungeonSize = new Vector2Int(generator.width, generator.height);

        ////conditions
        ///First, select a bunch of random points on the floortiles
        ///the start and end pos assets will be placed at room centers
        ///check a radius around each asset to ensure theres room to place it
        ///ensure there is a path using a pathfinding alg
        ///all corridor tiles must be unblocked

        List<Vector2> spawnPointsRaw = GeneratePoints(antiSpawnRadius, dungeonSize, numberOfSamplesBeforeRejection);
        //List<Vector2Int> actualPositions = new List<Vector2Int>();
        actualPositions.Clear();


		HashSet<Vector2Int> postions = new HashSet<Vector2Int>();
		foreach (Vector2 point in spawnPointsRaw)
		{
			Vector2Int pointInt = new Vector2Int((int)point.x, (int)point.y);

			int chance = Random.Range(0, 100);

			if (generatedFloorTiles != null && generatedFloorTiles.Contains(pointInt) && !generatedCorridors.Contains(pointInt) && chance <= density)
			{
				postions.Add(pointInt);
			}
		}

        actualPositions = postions;

        //---------------------------------------------------------------------------------------------------------------
        //now place stuff

        BoundsInt startRoom = generatedRooms[Random.Range(0, generatedRooms.Count)];
        BoundsInt endRoom = generatedRooms[Random.Range(0, generatedRooms.Count)];

        if(dstFromStartToEnd < generator.width && dstFromStartToEnd < generator.height)
        {
            while (Vector2.Distance(startRoom.center, endRoom.center) < dstFromStartToEnd)
            {
                startRoom = generatedRooms[Random.Range(0, generatedRooms.Count)];
                endRoom = generatedRooms[Random.Range(0, generatedRooms.Count)];
            }
        }
        else
        {
            Debug.Log("The defined distance between start and end is too big.");
        }

        if(printDst)
        {
            Debug.Log(Vector2.Distance(startRoom.center, endRoom.center).ToString());
        }
		

        while(startRoom == endRoom)
        {
			endRoom = generatedRooms[Random.Range(0, generatedRooms.Count)];
		}

        if(startPosAsset != null)
        {
            spawnedObjects.Add(Instantiate(startPosAsset, startRoom.center, Quaternion.identity));
        }

        if(dungeonExitAsset != null)
        {
            spawnedObjects.Add(Instantiate(dungeonExitAsset, endRoom.center, Quaternion.identity));
        }

        foreach(Vector2Int point in actualPositions)
        {
            if (point == new Vector2Int((int)startRoom.center.x, (int)startRoom.center.y) || point == new Vector2Int((int)endRoom.center.x, (int)endRoom.center.y))
            {
                continue;
            }

            Vector3 position = new Vector3(point.x + 0.5f, point.y + 0.5f, 0);
            int index = Random.Range(0, listOfAssetsToSpread.Count);
            GameObject obj = listOfAssetsToSpread[index];

            AssetDescripter data = obj.GetComponent<AssetDescripter>();
            if(data != null)
            {
				float rng = Random.Range(0, 101);
				if (rng <= data.spawnChance)
				{
					spawnedObjects.Add(Instantiate(obj, position, Quaternion.identity));
				}
			}
        }


        

		return true;
    }

	

	private List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesForRejection = 30)
    {
        float cellsize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellsize), Mathf.CeilToInt(sampleRegionSize.y / cellsize)];
        
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);

        while(spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];

            bool potentialPointAccepted = false;

            for(int i=0; i<numSamplesForRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 potentialPoint = spawnCenter * direction * Random.Range(radius, 2 * radius);

                if(IsValid(potentialPoint, sampleRegionSize, cellsize, radius, points, grid))
                {
                    points.Add(potentialPoint);
                    spawnPoints.Add(potentialPoint);
                    grid[(int)(potentialPoint.x / cellsize), (int)(potentialPoint.y / cellsize)] = points.Count;
                    potentialPointAccepted = true;
                    break;
                }
            }

            if(!potentialPointAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        return points;
    }

	private bool IsValid(Vector2 potentialPoint, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid) 
	{
		if(potentialPoint.x >= 0 && potentialPoint.x < sampleRegionSize.x && potentialPoint.y >= 0 && potentialPoint.y < sampleRegionSize.y)
        {
            int cellX = (int)(potentialPoint.x / cellSize);
            int cellY = (int)(potentialPoint.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX+2, grid.GetLength(0)-1);
			int searchStartY = Mathf.Max(0, cellY - 2);
			int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for(int x = searchStartX; x <= searchEndX; x++)
            {
                for(int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    if(pointIndex != -1)
                    {
                        float sqrDst = (potentialPoint - points[pointIndex]).sqrMagnitude;

                        if(sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
		}

        return false;
	}

	private void OnDrawGizmos()
	{
		if(showPoints && actualPositions.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach(Vector2Int position in actualPositions)
            {
                Gizmos.DrawSphere(new Vector3(position.x + 0.5f, position.y + 0.5f, 0), 0.5f);
            }
        }
	}
}
