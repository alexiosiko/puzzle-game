using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
	[SerializeField] AudioClip explosionClip;
	public Vector2 direction;
	public int maxDistance;
	int currentDistance;
	void OnEnable() => TurnManager.OnProjectilePhase += HandleOnProjectileTurn;
	void OnDisable() => TurnManager.OnProjectilePhase -= HandleOnProjectileTurn;
	public void Init(Vector2 direction, int maxDistance)
	{
		this.direction = direction;
		this.maxDistance = maxDistance;
		transform.right = direction;
	}
	bool firstMove = true;
	void HandleOnProjectileTurn()
	{
		if (currentDistance == maxDistance)
			Destroy(gameObject);

		if (firstMove == false && CheckHit(Vector2.zero) == true)
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
	void Explode()
	{
		AudioManager.Singleton.PlayClip(explosionClip);
		Destroy(gameObject);
	}
	bool CheckHit(Vector2 direction)
	{
		Vector2 newPos = (Vector2)transform.position + direction;
		var hit = Physics2D.OverlapPoint(newPos);
		if (hit)
		{
			if (hit.TryGetComponent(out Player p))
				TurnManager.Singleton.AddDie(p.Die());
			return true;
		}
		return false;
	}
	IEnumerator Move()
	{
		Vector2 newPos = (Vector2)transform.position + direction;
		currentDistance++;
		yield return transform.DOMove(newPos, GameSettings.tweenDuration).WaitForCompletion();
		firstMove = false;
	}
}
