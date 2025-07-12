using UnityEngine;

public class GridNode
{
	public Vector2Int position;
	public GridNode parent;
	public int gCost, hCost;
	public int fCost => gCost + hCost;
	public GridNode(Vector2Int pos)
	{
		position = pos;
	}
}
