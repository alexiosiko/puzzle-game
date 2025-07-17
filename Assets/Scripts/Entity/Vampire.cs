using System.Collections;
using UnityEngine;

public class Vampire : Enemy
{
	[SerializeField] GameObject fireballPrefab;
	GameObject currentFireBall;
	int maxDistance = 5;
	protected override void HandleOnEnemyMove()
	{
		if (currentFireBall)
			return; 

		if (CanAttackPlayer() == true)
			return;

		var next = GetNextMove(player.transform);
		if (next == null)
			return;
		TurnManager.Singleton.AddEnemy(Move((Vector2)next));


	}
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		CanAttackPlayer();
	}
	Vector2 direction;
	void SpawnFireball(Vector2 pos)
	{
		currentFireBall = Instantiate(fireballPrefab);
		currentFireBall.transform.position = pos;
		Projectile p = currentFireBall.GetComponent<Projectile>();
		p.Init(direction, maxDistance);

	}
	protected override bool CanAttackPlayer()
	{
		if (currentFireBall != null)
			return false;
			
		direction = GetDirectionToTarget(player.transform);
		if (direction.x != 0 && direction.y != 0)
			return false;
		if (CanShootPlayer(transform.position, direction) == true)
		{
			SpawnFireball((Vector2)transform.position);
			return true;
		}
		return false;
	}
	bool CanShootPlayer(Vector2 currentPos, Vector2 direction)
	{
		var stoppables = LayerMask.GetMask("Entity", "Wall");
		var hit = Physics2D.Raycast(currentPos, direction, maxDistance, stoppables);

		if (hit)
		{
			if (hit.collider.TryGetComponent(out Player p))
			{
				Debug.DrawLine(currentPos, hit.collider.transform.position, Color.cyan, 1f);
				return true;
			}

		}
		else
		{
			Debug.DrawLine(currentPos, currentPos + direction * maxDistance, Color.cyan, 1f);
		}
		return false;
	}
}
