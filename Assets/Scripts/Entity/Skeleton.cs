using System.Collections;
using UnityEngine;

public class Skeleton : Enemy
{
	protected override bool CanAttackPlayer()
	{
		foreach (var d in Utils.directions)
		{
			Vector2 pos = (Vector2)transform.position + d;
			var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
			if (hit && hit.TryGetComponent(out Player p))
			{

				TurnManager.Singleton.AddAttack(Attack(p));
				return true;
			}
		}
		return false;
	}
	

	protected override void HandleOnEnemyMove()
	{
		var next = GetNextMove(player.transform);
		if (next == null)
			return;
		if (CanAttackPlayer())
			return;
		TurnManager.Singleton.AddEnemy(Move((Vector2)next));
	}
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		CanAttackPlayer();
	}
}
