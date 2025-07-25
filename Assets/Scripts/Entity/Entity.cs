using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public abstract class Entity : SoundPlayer
{
	[SerializeField] protected AudioClip dieClip;
	[SerializeField] protected AudioClip[] footstepClips;
	public static HashSet<Vector2Int> reservedPositions = new();
	[SerializeField] protected LayerMask notWalkableLayers;
	protected bool WalkIntoEnemy(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
		if (hit && hit.TryGetComponent(out Enemy e))
			return true;
		return false;
	}
	bool CanMove(Vector2 pos)
	{
		if (WalkIntoEnemy(pos))
			return false;
		if (HitProjectile(pos))
			return false;
		return true;
	}
	protected virtual IEnumerator Move(Vector2 pos)
	{
		if (CanMove(pos) == false)
			yield break;
		PlayClip(footstepClips);
		FaceEntity(pos);
		yield return transform.DOMove(pos, GameSettings.tweenDuration).WaitForCompletion();
		HitProjectile(pos);

	}
	protected void FaceEntity(Vector2 target)
	{
		float x = target.x - transform.position.x;
		if (x < 0)
			transform.localScale = new(-1, 1, 1);
		else if (x > 0)
			transform.localScale = new(1, 1, 1);
	}
	protected bool HitProjectile(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Projectile"));
		if (hit)
		{
			hit.GetComponent<Projectile>().Explode();
			TurnManager.Singleton.AddDie(Die());
			return true;
		}
		return false;

	}
	public abstract IEnumerator Die();
	protected Animator animator;
	protected BoxCollider2D _collider;
	protected override void Awake()
	{
		_collider = GetComponent<BoxCollider2D>();
		base.Awake();
		animator = GetComponent<Animator>();

	}
	protected virtual void OnDestroy()
	{
		transform.DOKill();
		CancelInvoke();
	} 
}
