using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public abstract class Entity : SoundPlayer
{
	[SerializeField] protected AudioClip deathClip;
	[SerializeField] AudioClip[] footstepClips;
	protected virtual IEnumerator Move(Vector2 pos)
	{
		PlayAudio(footstepClips);
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
		yield return transform.DOScale(0, 0.5f).WaitForCompletion();
		Destroy(gameObject);
	}
	protected Animator animator;
	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}
}
