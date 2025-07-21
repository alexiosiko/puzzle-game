using UnityEngine;

public class Priest : Enemy
{
	protected override bool CanAttackPlayer()
	{
		return false;
	}

	protected override void HandleOnEnemyMove()
	{
		if (CanAttackPlayer())
		{
			SpawnArrow();
			return;
		}
		var next = GetNextMove(Vector2Int.RoundToInt(player.transform.position));
		if (next == null)
			return;
	}
	void SpawnArrow()
	{

	}
}
