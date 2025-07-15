using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class Enemy : Entity
{
	[SerializeField] AudioClip attackClip;
	[SerializeField] List<Vector2Int> path;
	public static HashSet<Vector2Int> reservedPositions = new();
	Player player;
	void HandleOnEnemyMove()
	{
		Vector2Int start = Vector2Int.FloorToInt(transform.position);
		Vector2Int target = Vector2Int.FloorToInt(player.transform.position);
		path = AStarPathfinder.FindPath(start, target);
		if (path == null)
		{
			// MoveRandom();
			return;
		}


		if (path.Count > 0)
		{
			Vector2Int next = path[0];

			// No double jumping!!
			if (reservedPositions.Contains(next))
				return;
			reservedPositions.Add(next);

			if (HitEntity(next))
				return;
			TurnManager.Singleton.AddEnemy(Move(next));
			
		}
	}
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		TryCanKillPlayer();
			
	}
	void TryCanKillPlayer()
	{
		foreach (var d in Utils.directions)
		{
			Vector2 pos = (Vector2)transform.position + d;
			var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
			if (hit && hit.TryGetComponent(out Player p))
			{
				TurnManager.Singleton.AddAttack(Attack(pos, p));
				return;
			}
		}
	}
	IEnumerator Attack(Vector2 targetPos, Player p)
	{
		FaceEntity(targetPos);
		PlayAudio(attackClip);
		animator.Play("Attack");
		yield return new WaitForSeconds(0.5f);
		yield return p.Die();
	}
	bool HitEntity(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
		if (hit)
		{
			if (hit.TryGetComponent(out Player p))
			{
				TurnManager.Singleton.AddAttack(Attack(pos, p));
				return true;
			}
			return true;
		}
		return false;
	}
	void MoveRandom()
	{
		Vector2[] directions = Utils.GetRandomDirections();

		Vector2 direction = Vector2.zero;
		for (int i = 0; i < directions.Length; i++)
		{
			if (CanMove(directions[i]))
			{
				direction = directions[i];
				break;
			}
		}

		if (direction != Vector2.zero)
			TurnManager.Singleton.AddEnemy(Move(direction + (Vector2)transform.position));
	}
	bool CanMove(Vector2 direction)
	{
		Vector2 pos = (Vector2)transform.position + direction;
		var hit = Physics2D.OverlapPoint(pos + GameSettings.rayCastOffset, GameSettings.notWalkableLayers);
		if (hit)
			return false;
		return true;
	}

	void OnEnable() => TurnManager.OnEnemyPhase += HandleOnEnemyMove;
	void OnDisable() => TurnManager.OnEnemyPhase -= HandleOnEnemyMove;
	static void OnDestroy()
	{
		reservedPositions.Clear();
	}  
	protected override void Awake()
	{
		base.Awake();
		player = FindFirstObjectByType<Player>();
	}

}
