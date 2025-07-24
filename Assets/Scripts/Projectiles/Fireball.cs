using System.Collections;
using DG.Tweening;
using UnityEngine;
public class Fireball : Projectile
{
	public override void Init(Vector2 direction, int maxDistance)
	{
		base.Init(direction, maxDistance);
		transform.DOMove((Vector2)transform.position + direction, GameSettings.tweenDuration);
		if (CheckHit(direction))
			Explode();
	}
	protected override void HandleOnProjectileTurn()
	{
		if (firstMove == false && CheckHit(Vector2.zero) == true)
		{
			firstMove = false;
			Explode();
			return;
		}

		if (currentDistance == maxDistance)
		{
			Explode();
			return;
		}

		if (CheckHit(direction) == true)
		{
			Explode();
			return;
		}


		TurnManager.Singleton.AddProjectile(Move());
	}

	protected override IEnumerator Move()
	{
		Vector2 newPos = (Vector2)transform.position + direction;
		currentDistance++;
		yield return transform.DOMove(newPos, GameSettings.tweenDuration).WaitForCompletion();
		firstMove = false;
		CheckHit(Vector2.zero);
	}
}
