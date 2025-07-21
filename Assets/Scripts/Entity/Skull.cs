using System.Collections;
using UnityEngine;
public class Skull : Enemy
{

	protected override void HandleOnEnemyMove()
	{
		if (CanAttackPlayer())
		{
			TurnManager.Singleton.AddAttack(Attack(player));
			return;
		}

		var next = GetNextMove(Vector2Int.RoundToInt(player.transform.position));
		if (next == null)
			return;



		TurnManager.Singleton.AddEnemy(Move((Vector2)next));
	}
	protected override bool CanAttackPlayer()
	{
		foreach (var d in Utils.directions)
		{
			Vector2 nextPos = (Vector2)transform.position + d;
			var hit = Physics2D.OverlapPoint(nextPos, LayerMask.GetMask("Entity"));
			if (hit && hit.TryGetComponent(out Player p))
			{
				TurnManager.Singleton.AddAttack(Attack(p));
				return true;
			}
		}
		return false;
	}
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		CanAttackPlayer();
	}
}
