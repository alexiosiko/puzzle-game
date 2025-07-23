using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Arrow : Projectile
{
	protected override void HandleOnProjectileTurn() => StartCoroutine(Move());
	public IEnumerator GetMove() => Move();
	protected override IEnumerator Move()
	{

		Player player = FindFirstObjectByType<Player>();
		int totalSpaces = Mathf.RoundToInt((player.transform.position - transform.position).magnitude);
		print(totalSpaces);
		while (totalSpaces > 0)
		{
			totalSpaces--;
			yield return transform.DOMove((Vector2)transform.position + direction, GameSettings.tweenDuration / 2f).SetEase(Ease.Linear).WaitForCompletion();
		}
		Destroy(gameObject);
		yield return player.Die();
	}
}
