using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public abstract class Entity : SoundPlayer
{
	[SerializeField] protected AudioClip dieClip;
	[SerializeField] AudioClip[] footstepClips;
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
		yield return transform.DOMove(pos, GameSettings.tweenDuration).WaitForCompletion();
	}
	protected void FaceEntity(Vector2 target)
	{
		float x = target.x - transform.position.x;
		if (x < 0)
			transform.localScale = new(-1, 1, 1);
		else if (x > 0)
			transform.localScale = new(1, 1, 1);
	}
	public virtual IEnumerator Die()
	{
		yield return transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		Destroy(gameObject);
	}
	protected Animator animator;
	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();

	}
	protected virtual void OnDestroy()
	{
		reservedPositions.Clear();
	} 
}
