using System.Collections;
using UnityEngine;

public class Vampire : Enemy
{
	[SerializeField] GameObject fireballPrefab;
	[SerializeField] GameObject currentFireBall;
	int maxDistance = 5;
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

		var next = GetNextMove(player.transform);
		if (next == null)
			return;
		TurnManager.Singleton.AddEnemy(Move((Vector2)next));


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
		waitOneTurn = true;
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
		Vector2 currentPos = (Vector2)transform.position;
		var stoppables = LayerMask.GetMask("Wall", "Moveable", "Entity");
		var hit = Physics2D.Raycast(currentPos + direction, direction, maxDistance, stoppables);
  
		if (hit)
		{
			if (hit.collider.TryGetComponent(out Player p))
			{
				Debug.DrawLine(currentPos + direction, hit.collider.transform.position, Color.green, 1f);
				return true;
			}
			Debug.Log(hit.collider.name);
			Debug.DrawLine(currentPos + direction, currentPos + direction * maxDistance, Color.orange, 1f);
		}
		else
		{
			Debug.DrawLine(currentPos + direction, currentPos + direction * maxDistance, Color.red, 1f);
		}
		return false;
	}
}
