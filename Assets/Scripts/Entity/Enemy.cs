using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Enemy : Entity
{
	[SerializeField] protected AudioClip attackClip;
	protected Player player;
	protected abstract void HandleOnEnemyMove();
	protected abstract bool CanAttackPlayer();
	[SerializeField] List<Vector2Int> path;
	protected Vector2Int? GetNextMove(Transform targetTrasform)
	{

		Vector2Int start = Vector2Int.FloorToInt(transform.position);
		Vector2Int target = Vector2Int.FloorToInt(targetTrasform.position);
		path = AStarPathfinder.FindPath(start, target, notWalkableLayers);
		if (path == null || path.Count == 0)
			return null;
		Vector2Int pos = path[0];
		if (reservedPositions.Contains(pos))
			return null;
		reservedPositions.Add(pos);
		return pos;
	}

	protected IEnumerator Attack(Player p)
	{
		FaceEntity(p.transform.position);
		PlayAudio(attackClip);
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
	protected Vector2 GetDirectionToTarget(Transform target) => (target.position - transform.position).normalized;
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
