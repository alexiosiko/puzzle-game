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
	protected Entity HitEntity(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Entity"));
		if (hit)
			return hit.GetComponent<Entity>();
		return null;
	}
	protected virtual IEnumerator Move(Vector2 pos)
	{
		PlayClips(footstepClips);
		FaceEntity(pos);
		if (HitProjectile(pos))
		{
			yield return transform.DOMove(pos, GameSettings.tweenDuration).WaitForCompletion();
			yield break;
		}
		else
		{
			yield return transform.DOMove(pos, GameSettings.tweenDuration).WaitForCompletion();
			HitProjectile(pos);
		}
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
