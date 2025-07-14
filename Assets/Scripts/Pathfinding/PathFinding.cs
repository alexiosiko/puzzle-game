using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class AStarPathfinder
{
	static int maxX = 20;
	static int maxY = 20;
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
	{
		Dictionary<Vector2Int, GridNode> allNodes = new();
		List<GridNode> openSet = new();
		HashSet<Vector2Int> closedSet = new();

		GridNode startNode = new(start);
		startNode.gCost = 0;
		startNode.hCost = Heuristic(start, end);
		openSet.Add(startNode);
		allNodes[start] = startNode;

		while (openSet.Count > 0)
		{
			GridNode current = GetLowestFCostNode(openSet);

			if (current.position == end)

				return RetracePath(current);
			

			openSet.Remove(current);
			closedSet.Add(current.position);

			foreach (Vector2Int dir in Directions)
			{
				Vector2Int neighborPos = current.position + dir;
				if (!IsInBounds(neighborPos) || !IsWalkable(neighborPos) || closedSet.Contains(neighborPos))
					continue;

				int newCost = current.gCost + 1;
				if (!allNodes.TryGetValue(neighborPos, out GridNode neighbor))
				{
					neighbor = new GridNode(neighborPos);
					allNodes[neighborPos] = neighbor;
				}

				if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
				{
					neighbor.gCost = newCost;
					neighbor.hCost = Heuristic(neighborPos, end);
					neighbor.parent = current;

					if (!openSet.Contains(neighbor))
						openSet.Add(neighbor);
				}
			}
		}
		return null; // No path found
	}

	static bool IsWalkable(Vector2Int pos)
	{

		RaycastHit2D hit = Physics2D.Raycast(pos + GameSettings.rayCastOffset, Vector2.zero, 0.1f, GameSettings.notWalkableLayers);
		if (hit.collider)
		{
			// If hit entity, but it's a player, treat as walkable
			if (hit.collider.TryGetComponent(out Player p))
				return true;
			return false;
		}
		return true;
    }

	static bool IsInBounds(Vector2Int pos) => pos.x >= -maxX && pos.y >= -maxX && pos.x < maxX && pos.y < maxY;

    static List<Vector2Int> RetracePath(GridNode endNode)
    {
        List<Vector2Int> path = new();
        GridNode current = endNode;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();

		// Remove starting node
		path.RemoveAt(0);

        return path;
    }

    static GridNode GetLowestFCostNode(List<GridNode> nodes)
    {
        GridNode best = nodes[0];
        foreach (var node in nodes)
        {
            if (node.fCost < best.fCost || (node.fCost == best.fCost && node.hCost < best.hCost))
                best = node;
        }
        return best;
    }

    static readonly Vector2Int[] Directions = {
        Vector2Int.up, Vector2Int.down,
        Vector2Int.left, Vector2Int.right
    };

    static int Heuristic(Vector2Int a, Vector2Int b) =>
        Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
}
