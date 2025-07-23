using System.Collections;
using UnityEngine;

public class Skeleton : Enemy
{
	protected override bool CanAttackPlayer()
	{
		foreach (var d in Utils.directions)
		{
			Vector2 nextPos = (Vector2)transform.position + d;
			var hit = Physics2D.OverlapPoint(nextPos, LayerMask.GetMask("Player"));
			if (hit && hit.TryGetComponent(out Player p))
				return true;
		}
		return false;
	}
	

	protected override void HandleOnEnemyMove()
	{
		var next = GetNextMove(Vector2Int.RoundToInt(player.transform.position));
		if (next == null)
			return;
		if (CanAttackPlayer())
		{
			TurnManager.Singleton.AddAttack(Attack(player));
			return;
		}
		TurnManager.Singleton.AddEnemy(Move((Vector2)next));
	}
	
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		CanAttackPlayer();
	}
}
