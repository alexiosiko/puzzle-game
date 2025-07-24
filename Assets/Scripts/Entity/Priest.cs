using System.Collections;
using UnityEngine;

public class Priest : Enemy
{
	int maxShootDistance = 15;
	[SerializeField] GameObject arrowPrefab;
	GameObject currentArrow;
	int shootableLayers;
	protected override bool CanAttackPlayer()
	{
		direction = GetDirectionToTarget(player.transform);
		Vector2 currentPos = (Vector2)transform.position + direction;

		if (direction.x != 0 && direction.y != 0)
		{
			Debug.DrawLine(currentPos, currentPos + direction * maxShootDistance, Color.red, 1f);
			return false;
		}
		var hit = Physics2D.Raycast(currentPos, direction, maxShootDistance, shootableLayers);
		if (hit)
		{
				if (hit.collider.TryGetComponent(out Player p))
			{
				Debug.DrawLine(currentPos, hit.collider.transform.position, Color.green, 1f);
				return true;
			}
			Debug.DrawLine(currentPos, currentPos + direction * maxShootDistance, Color.orange, 1f);
		}
		else
		{
			Debug.DrawLine(currentPos, currentPos + direction * maxShootDistance, Color.red, 1f);
		}
		return false;
	}
	bool waitOneTurn = false;
	protected override void HandleOnEnemyMove()
	{
		if (waitOneTurn == true)
		{
			waitOneTurn = false;
			return;
		}

		if (CanAttackPlayer())
		{
			TurnManager.Singleton.AddAttack(SpawnArrow((Vector2)transform.position));
			return;
		}
		var next = GetNextMove(Vector2Int.RoundToInt(player.transform.position));
		if (next == null)
			return;

		TurnManager.Singleton.AddEnemy(Move((Vector2)next));
	}
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		if (CanAttackPlayer() == true)
			TurnManager.Singleton.AddAttack(SpawnArrow(pos + direction));
	}
	IEnumerator SpawnArrow(Vector2 pos)
	{
		animator.Play("Attack");
		IEnumerator function = SpawnArrowWithHasedCode(pos);
		attackHashedCode = function.GetHashCode();
		return function;
	}
	IEnumerator SpawnArrowWithHasedCode(Vector2 pos)
	{
		PlayClip(attackClip);
		animator.Play("Attack");
		waitOneTurn = true;
		currentArrow = Instantiate(arrowPrefab);
		currentArrow.transform.position = pos;
		FaceEntity(direction + pos);
		yield return new WaitForSeconds(0.1f);
		Arrow p = currentArrow.GetComponent<Arrow>();
		p.Init(direction, maxShootDistance);
		yield return p.GetMove();
	}
	Vector2 direction;
	protected override void Awake()
	{
		base.Awake();
		shootableLayers = arrowPrefab.GetComponent<Projectile>().hitLayers;
	}
}
