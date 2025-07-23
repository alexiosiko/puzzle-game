 using System.Collections;
using System.IO;
using UnityEngine;

public class Vampire : Enemy
{
	[SerializeField] GameObject fireballPrefab;
	GameObject currentFireBall;
	int maxDistance = 15;
	bool waitOneTurn = false;
	protected override void HandleOnEnemyMove()
	{
		if (waitOneTurn == true)
		{
			waitOneTurn = false;
			return;
		}
		if (CanAttackPlayer() == true)
		{

			SpawnFireball((Vector2)transform.position);
			return;
		}


		var next = GetNextMove(Vector2Int.RoundToInt(player.transform.position));

		if (next == null)
			return;

		if (WalkIntoPlayer((Vector2)next))
			return;

		TurnManager.Singleton.AddEnemy(Move((Vector2)next));
	}
	bool WalkIntoPlayer(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
		if (hit && hit.TryGetComponent(out Player p))
			return true;
		return false;
	}
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		if (CanAttackPlayer() == true)
			SpawnFireball(pos);
	}
	Vector2 direction;
	void SpawnFireball(Vector2 pos)
	{
		PlayClip(attackClip);
		animator.Play("Attack");
		waitOneTurn = true;
		currentFireBall = Instantiate(fireballPrefab);
		currentFireBall.transform.position = pos;
		FaceEntity(direction + pos);
		Projectile p = currentFireBall.GetComponent<Projectile>();
		p.Init(direction, maxDistance);

	}
	LayerMask cannotShootThrough;
	protected override bool CanAttackPlayer()
	{
		if (currentFireBall != null)
			return false;

		direction = GetDirectionToTarget(player.transform);
		if (direction.x != 0 && direction.y != 0)
			return false;
		Vector2 currentPos = (Vector2)transform.position;
		Debug.DrawLine(currentPos + direction, currentPos + direction * maxDistance, Color.red, 1f);

		var hit = Physics2D.Raycast(currentPos + direction, direction, maxDistance, cannotShootThrough);
		if (hit)
		{
			if (hit.collider.TryGetComponent(out Player p))
			{
				Debug.DrawLine(currentPos + direction, hit.collider.transform.position, Color.green, 1f);
				return true;
			}
			Debug.DrawLine(currentPos + direction, currentPos + direction * maxDistance, Color.orange, 1f);
		}
		else
		{
			Debug.DrawLine(currentPos + direction, currentPos + direction * maxDistance, Color.red, 1f);
		}
		return false;
	}
	protected override void Awake()
	{
		base.Awake();
		cannotShootThrough = fireballPrefab.GetComponent<Projectile>().hitLayers;
	}
}
