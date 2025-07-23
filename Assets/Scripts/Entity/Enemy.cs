using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public abstract class Enemy : Entity
{
	public override IEnumerator Die()
	{
		_collider.enabled = false;
		animator.Play("Die");
		enabled = false;
		Destroy(gameObject, 1f);
		yield return new WaitForSeconds(GameSettings.tweenDuration);
	}
	[SerializeField] protected AudioClip attackClip;
	[HideInInspector] public Player player;
	protected abstract void HandleOnEnemyMove();
	protected abstract bool CanAttackPlayer();
	[SerializeField] List<Vector2Int> path;

	protected Vector2Int? GetNextMove(Vector2Int target)
	{

		Vector2Int start = Vector2Int.RoundToInt(transform.position);
		path = AStarPathfinder.FindPath(start, target, notWalkableLayers);
		if (path == null || path.Count == 0)
			return null;
		Vector2Int pos = path[0];

		// Could want into entity cause we said we can walk through entites in path finding
		if (WalkIntoEnemy(pos))
			return null;

		if (reservedPositions.Contains(pos))
			return null;
		reservedPositions.Add(pos);


		return pos;
	}  
	
	
	bool WalkIntoEnemy(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
		if (hit && hit.TryGetComponent(out Enemy e))
			return true;
		return false;
	}
	[HideInInspector] public int attackHashedCode;
	protected virtual IEnumerator Attack(Player p)
	{
		animator.Play("Attack");
		IEnumerator function = AttackWithHashCode(p);
		attackHashedCode = function.GetHashCode();
		return function;
	}
	IEnumerator AttackWithHashCode(Player p)
	{
		FaceEntity(p.transform.position);
		PlayClip(attackClip);
		animator.Play("Attack");
		yield return new WaitForSeconds(0.5f);
		yield return p.Die();
	}
	protected virtual void OnEnable() => TurnManager.OnEnemyPhase += HandleOnEnemyMove;
	protected virtual void OnDisable() => TurnManager.OnEnemyPhase -= HandleOnEnemyMove;
	protected override void Awake()
	{
		base.Awake();
		player = FindFirstObjectByType<Player>();
	}
	protected Vector2 GetDirectionToTarget(Transform target)
	{
		Vector2 delta = Vector2Int.RoundToInt(target.position) - Vector2Int.RoundToInt(transform.position);
		return delta.normalized;
	} 
	// void MoveRandom()
	// {
	// 	Vector2[] directions = Utils.GetRandomDirections();

	// 	Vector2 direction = Vector2.zero;
	// 	for (int i = 0; i < directions.Length; i++)
	// 	{
	// 		if (CanMove(directions[i]))
	// 		{
	// 			direction = directions[i];
	// 			break;
	// 		}
	// 	}

	// 	if (direction != Vector2.zero)
	// 		TurnManager.Singleton.AddEnemy(Move(direction + (Vector2)transform.position));
	// }
	// bool CanMove(Vector2 direction)
	// {
	// 	Vector2 pos = (Vector2)transform.position + direction;
	// 	var hit = Physics2D.OverlapPoint(pos + GameSettings.rayCastOffset, GameSettings.notWalkableLayers);
	// 	if (hit)
	// 		return false;
	// 	return true;
	// }
}
