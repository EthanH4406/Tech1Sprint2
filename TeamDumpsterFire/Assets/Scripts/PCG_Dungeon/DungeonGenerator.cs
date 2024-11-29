using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.NetworkInformation;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Security.Cryptography;

public class DungeonGenerator : MonoBehaviour
{
	[Header("General Settings")]
	public Tilemap floorTilemap;
    public Tilemap wallsTilemap;
    public bool layerAutomata;

    [Header("Debug Settings")]
    public bool showAutomata;
    public bool showRooms;
    public bool showUnCulledTiles;
    public bool showCorridors;

	//CA
	[Header("Cellular Automata Settings")]
	[Range(0, 100)]
    public int noiseDensity = 50;
    public int width;
    public int height;
    public int smoothPasses = 5;

    public string seed;
    public bool useRandomSeed;

    private int[,] map_CA;

    //BSP Rooms
    [Header("Binary Space Partitioning Settings")]
    [SerializeField]
    private int minRoomWidth = 4;
	[SerializeField]
	private int minRoomHeight = 4;
	[SerializeField]
	private int bspWidth = 20;
	[SerializeField]
	private int bspHeight = 20;

	[SerializeField]
    [Range(0, 10)]
	private int offset = 1;
    public Vector2Int startPos;

    [Header("Tiles")]
    public TileBase floorTile;
    public TileBase wallTop;
    public TileBase wallSideRight;
    public TileBase wallSideLeft;
    public TileBase wallBottom;
    public TileBase wallFull;
    public TileBase wallInnerCornerDownLeft;
    public TileBase wallInnerCornerDownRight;
    public TileBase wallDiagonalCornerDownRight;
    public TileBase wallDiagonalCornerDownLeft;
    public TileBase wallDiagonalCornerUpRight;
    public TileBase wallDiagonalCornerUpLeft;

    private List<BoundsInt> createdRooms = new List<BoundsInt>();
	private List<Vector2Int> RemovedTiles = new List<Vector2Int>();
    private HashSet<Vector2Int> CreatedCorridors = new HashSet<Vector2Int>();

	// Start is called before the first frame update
	void Start()
    {
        //GenerateCAMap();
        GenerateBSPMap();
        //BuildMap();
        //EnsureConnectivity();
        //PopulateMap();
        //CheckForPath();

        ////
        ///Generate random blotches using CA
        ///Layer BSP on top
        ///Connect BSP with hallways
        ///Use Flood Fill to connect CA segments with BSP
        ///Place tiles according to the defined rules
        ///Identify and Create a List of all rooms (include a id, type, bounds, and pos for the room)
        ///Populate each room with assets (in accordance to type)
        ///Run Flood Fill again to ensure connectivity 
        ///     Optionally instead, run a pathfinding algor to make sure there exists a path from player start to end
    }

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
        {
            Clear();
            GenerateBSPMap();
        }
	}

	private void GenerateBSPMap()
	{
        CreateRooms();
	}

	private void CreateRooms()
	{
        List<BoundsInt> roomsList = BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos, new Vector3Int(bspWidth, bspHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);

        createdRooms = roomsList;

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach(BoundsInt room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

		HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        CreatedCorridors = corridors;
		floor.UnionWith(corridors);

		if (layerAutomata)
        {
			GenerateCAMap();
			HashSet<Vector2Int> automata = GetAutomataAsGrid();
			floor.UnionWith(automata);
		}

        //Remove disconnected areas
        Vector2Int checkPos = new Vector2Int((int)roomsList[0].center.x, (int)roomsList[0].center.y);
        floor = ConvertMap(CullMap(checkPos, floor));

        //paint floors
        PaintFloorTiles(floor);
        //paint walls
        CreateWalls(floor);
	}

    private HashSet<Vector2Int> ConvertMap(List<Vector2Int> rawMap)
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        foreach(Vector2Int tile in rawMap)
        {
            map.Add(tile);
        }

        return map;
    }

    private List<Vector2Int> CullMap(Vector2Int startPos, HashSet<Vector2Int> floorTiles)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
		int[,] mapFlags = new int[width, height];


		Queue<Vector2Int> tileQueue = new Queue<Vector2Int>();
        tileQueue.Enqueue(startPos);

        mapFlags[startPos.x, startPos.y] = 1;

        int[,] map = new int[width, height];

        for(int i=0; i < width; i++)
        {
            for(int j=0; j < height; j++)
            {
                map[i, j] = 1;
            }
        }

		foreach (Vector2Int pos in floorTiles)
		{
			map[pos.x, pos.y] = 0;
		}


		while (tileQueue.Count > 0)
        {
            Vector2Int currentTile = tileQueue.Dequeue();
            tiles.Add(currentTile);

            for(int x = currentTile.x - 1; x<= currentTile.x + 1; x++)
            {
				for (int y = currentTile.y - 1; y <= currentTile.y + 1; y++)
                {
                    if(IsInMapRange(currentTile) && (y == currentTile.y || x == currentTile.x))
                    {
                        if (mapFlags[x,y] == 0 && map[x,y] == 0)
                        {
                            mapFlags[x,y] = 1;
                            tileQueue.Enqueue(new Vector2Int(x, y));
                        }
                    }
                }

			}

		}

        return tiles;
    }

    private bool IsInMapRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }


	private HashSet<Vector2Int> GetAutomataAsGrid()
	{
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
                if (map_CA[x,y] == 0)
                {
                    floor.Add(new Vector2Int(x,y));
                }
			}
		}

        return floor;
	}

	private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        Vector2Int currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while(roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int roomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        Vector2Int position = roomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        return corridor;
    }

	private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
	{
		Vector2Int closest = Vector2Int.zero;
		float distance = float.MaxValue;
		foreach (var position in roomCenters)
		{
			float currentDistance = Vector2.Distance(position, currentRoomCenter);
			if (currentDistance < distance)
			{
				distance = currentDistance;
				closest = position;
			}
		}
		return closest;
	}

	private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
	{
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach(BoundsInt room in roomsList)
        {
            //room
            for(int col = offset; col < room.size.x - offset; col++)
            {
				for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
			}
        }

        return floor;
	}

	public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while(roomsQueue.Count > 0)
        {
            BoundsInt room = roomsQueue.Dequeue();
            if(room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if(UnityEngine.Random.value < 0.5f)
                {
                    if(room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if(room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
					}
                    else if(room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
					if (room.size.x >= minWidth * 2)
					{
						SplitVertically(minWidth, roomsQueue, room);
					}
                    else if(room.size.y >= minHeight * 2)
                    {
						SplitHorizontally(minHeight, roomsQueue, room);
					}
					else if (room.size.x >= minWidth && room.size.y >= minHeight)
					{
						roomsList.Add(room);
					}
				}
            }
        }

        return roomsList;
    }

	private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
	{
        int xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
		BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
	}

	private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
	{
        int ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
	}

    private void GenerateCAMap()
    {
        map_CA = new int[width, height];
        RandomNoiseGrid();

        //5 is smooth passes. expose it
        for(int i=0; i<smoothPasses; i++)
        {
            SmoothMap();
        }

    }

    private void RandomNoiseGrid()
    {
        if(useRandomSeed)
        {
			string str = "abcdefghijklmnopqrstuvwxyz0123456789";
			int size = 8;

			string randomstring = "";

			for (int i = 0; i < size; i++)
			{
				int x = Random.Range(0, str.Length);

				randomstring = randomstring + str[x];
			}

			seed = randomstring;
        }

        System.Random prng = new System.Random(seed.GetHashCode());

        for(int x=0; x < width; x++)
        {
            for(int y=0; y < height; y++)
            {
                if(x==0 || x==width-1 || y==0 || y==height-1)
                {
                    map_CA[x, y] = 1;
				}
                else
                {
					map_CA[x, y] = (prng.Next(0, 100) < noiseDensity) ? 1 : 0;
				}
            }
        }
    }

    private void SmoothMap()
    {
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int nWallTiles = GetSurroundingWallCount(x, y);

                if(nWallTiles > 4)
                {
                    map_CA[x, y] = 1;
                }
                else if(nWallTiles < 4)
                {
                    map_CA[x, y] = 0;
                }
			}
		}
	}

    private int GetSurroundingWallCount(int gridx, int gridy)
    {
        int wallCount = 0;

        for(int neighborX = gridx-1; neighborX <= gridx+1; neighborX++)
        {
			for (int neighborY = gridy - 1; neighborY <= gridy + 1; neighborY++)
            {
                if(neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
					if (neighborX != gridx || neighborY != gridy)
					{

						wallCount += map_CA[neighborX, neighborY];
					}
				}
                else
                {
                    wallCount++;
                }
            }

		}

        return wallCount;
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(Vector2Int pos in positions)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePosition, tile);
    }

    private void PaintSingleBasicWall(Vector2Int pos, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

		if (WallTypesHelper.wallTop.Contains(typeAsInt))
		{
			tile = wallTop;
		}
		else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
		{
			tile = wallSideRight;
		}
		else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
		{
			tile = wallSideLeft;
		}
		else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
		{
			tile = wallBottom;
		}
		else if (WallTypesHelper.wallFull.Contains(typeAsInt))
		{
			tile = wallFull;
		}

		if (tile != null)
        {
            PaintSingleTile(wallsTilemap, tile, pos);
        }
			
	}

	private void PaintSingleCornerWall(Vector2Int position, string binaryType)
	{
		int typeASInt = Convert.ToInt32(binaryType, 2);
		TileBase tile = null;

		if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
		{
			tile = wallInnerCornerDownLeft;
		}
		else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
		{
			tile = wallInnerCornerDownRight;
		}
		else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
		{
			tile = wallDiagonalCornerDownLeft;
		}
		else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
		{
			tile = wallDiagonalCornerDownRight;
		}
		else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
		{
			tile = wallDiagonalCornerUpRight;
		}
		else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
		{
			tile = wallDiagonalCornerUpLeft;
		}
		else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
		{
			tile = wallFull;
		}
		else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
		{
			tile = wallBottom;
		}

		if (tile != null)
        {
			PaintSingleTile(wallsTilemap, tile, position);
		}
	}

    private void CreateWalls(HashSet<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, cardinalDirectionsList);
        HashSet<Vector2Int> diagonalWallPositions = FindWallsInDirections(floorPositions, diagonalDirectionsList);
        CreateBasicWall(basicWallPositions, floorPositions);
        CreateCornerWalls(diagonalWallPositions, floorPositions);
    }

    private void CreateCornerWalls(HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach(Vector2Int pos in cornerWallPositions)
        {
            string neighboursBinaryType = "";

            foreach(Vector2Int direction in eightDirectionsList)
            {
                Vector2Int neighbourPosition = pos + direction;

                if(floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            PaintSingleCornerWall(pos, neighboursBinaryType);

		}
    }

	private void CreateBasicWall(HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
	{
		foreach (var position in basicWallPositions)
		{
			string neighboursBinaryType = "";
			foreach (var direction in cardinalDirectionsList)
			{
				var neighbourPosition = position + direction;
				if (floorPositions.Contains(neighbourPosition))
				{
					neighboursBinaryType += "1";
				}
				else
				{
					neighboursBinaryType += "0";
				}
			}
			PaintSingleBasicWall(position, neighboursBinaryType);
		}
	}

	private HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
	{
		HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
		foreach (var position in floorPositions)
		{
			foreach (var direction in directionList)
			{
				var neighbourPosition = position + direction;
				if (floorPositions.Contains(neighbourPosition) == false)
					wallPositions.Add(neighbourPosition);
			}
		}
		return wallPositions;
	}

	private List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
	{
		new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) //LEFT
    };

	private List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
	{
		new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

	private List<Vector2Int> eightDirectionsList = new List<Vector2Int>
	{
		new Vector2Int(0,1), //UP
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(-1, 1) //LEFT-UP

    };

	public Vector2Int GetRandomCardinalDirection()
	{
		return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
	}

	public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallsTilemap.ClearAllTiles();
    }

	private void OnDrawGizmos()
	{

		if(map_CA != null && showAutomata)
        {
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
                    if (map_CA[x,y] == 1)
                    {
                        Gizmos.color = Color.black;
                    }
                    else if(map_CA[x,y] == 0)
                    {
                        Gizmos.color = Color.white;
                    }

                    Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one);
				}
			}
		}

        if(showRooms && createdRooms.Count > 0)
        {
            Gizmos.color = Color.blue;

            foreach(BoundsInt room in createdRooms)
            {
                Gizmos.DrawWireCube(room.center, room.size);
            }
        }

        if(showUnCulledTiles && RemovedTiles.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach(Vector2Int tile in RemovedTiles)
            {
                Gizmos.DrawWireCube(new Vector3(tile.x, tile.y, 0), Vector3.one);
            }
        }

        if(showCorridors && CreatedCorridors.Count > 0)
        {
			Gizmos.color = Color.yellow;
			foreach (Vector2Int tile in CreatedCorridors)
			{
				Gizmos.DrawWireCube(new Vector3(tile.x, tile.y, 0), Vector3.one);
			}
		}
	}
}
