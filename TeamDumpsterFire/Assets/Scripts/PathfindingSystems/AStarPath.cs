using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarPath
{
    public Dictionary<PathNode, PathNode> cameFrom = new Dictionary<PathNode, PathNode>();
    public Dictionary<PathNode, double> costSoFar = new Dictionary<PathNode, double>();

    public static double Heuristic(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public AStarPath(Vector2[,] graph, PathNode startPos, PathNode targetPos)
    {
        PriorityQueue<PathNode, double> frontier = new PriorityQueue<PathNode, double>();
        frontier.Enqueue(startPos, 0);

        cameFrom[startPos] = startPos;
        costSoFar[startPos] = 0;

        while(frontier.Count > 0)
        {
            PathNode current = frontier.Dequeue();

            if(current.Equals(targetPos))
            {
                break;
            }
        }
	}

    
}


