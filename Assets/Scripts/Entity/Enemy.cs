using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class Enemy : Entity
{
	[SerializeField] AudioClip attackClip;
	[SerializeField] List<Vector2Int> path;
	[SerializeField] Vector2Int targetPosition;
	public static HashSet<Vector2Int> reservedPositions = new();
	Player player;
	void HandleOnEnemyMove()
	{
		Vector2Int start = Vector2Int.FloorToInt(transform.position);
		Vector2Int target = Vector2Int.FloorToInt((Vector2)player.transform.position);
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
			TurnManager.AddEnemy(Move(next));
			
		}
	}
	protected override IEnumerator Move(Vector2 pos)
	{
		StartCoroutine(base.Move(pos));
		yield return new WaitForSeconds(GameSettings.tweenDuration / 2f);
		if (CanKillPlayer())
			yield return KillPlayer();
	}
	bool CanKillPlayer()
	{
		foreach (var d in Utils.directions)
		{
			Vector2 pos = (Vector2)transform.position + d;
			var hit = Physics2D.Raycast(pos, Vector2.zero, 10f, LayerMask.GetMask("Entity"));
			if (hit.collider && hit.collider.TryGetComponent(out Player p))
				return true;
		}
		return false;
	}
	IEnumerator KillPlayer()
	{
		Play(attackClip);
		animator.Play("Attack");
		yield return new WaitForSeconds(1);
		Player.OnPlayerDie?.Invoke();
	}
	bool HitEntity(Vector2 pos)
	{
		var hit = Physics2D.Raycast(pos, Vector2.zero, 0, LayerMask.GetMask("Entity"));
		if (hit.collider)
		{
			if (hit.collider.TryGetComponent(out Player p))
			{
				TurnManager.Singleton.StopAllCoroutines();
				StartCoroutine(KillPlayer());
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
			TurnManager.AddEnemy(Move(direction + (Vector2)transform.position));
	}
	bool CanMove(Vector2 direction)
	{
		Vector2 pos = (Vector2)transform.position + direction;
		var hit = Physics2D.Raycast(pos + GameSettings.rayCastOffset, Vector2.zero, 10f, GameSettings.notWalkableLayers);
		if (hit.collider)
			return false;
		return true;
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			Vector2Int start = Vector2Int.FloorToInt(transform.position);
			path = AStarPathfinder.FindPath(start, targetPosition);
		}
	}
	void OnEnable() => TurnManager.OnEnemyPhase += HandleOnEnemyMove;
	void OnDisable() => TurnManager.OnEnemyPhase -= HandleOnEnemyMove;
	protected override void Awake()
	{
		base.Awake();
		player = FindFirstObjectByType<Player>();
	}

}
