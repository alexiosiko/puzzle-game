using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Moveable : MonoBehaviour
{
	[SerializeField] AudioClip onMoveClip;
	public bool CanMove(Vector2 direction)
	{
		if (direction == Vector2.zero)
			return false;
		Vector2 targetPos = (Vector2)transform.position + direction;
		// if (targetPos)

		// Ignore self and check target
		var hit = Physics2D.OverlapPoint(targetPos + GameSettings.rayCastOffset);

		if (!hit)
		{
			Move(direction);
			return true;
		}
		if (hit.TryGetComponent(out Projectile p))
		{
			p.Explode();
			Move(direction);
			return true;
		}

		// If another moveable, check recursively
		if (hit.TryGetComponent(out Moveable m))
		{
			bool canMoveOtherMoveable = m.CanMove(direction);
			if (canMoveOtherMoveable)
				Move(direction);
			return canMoveOtherMoveable;

		}

		// Blocked
		return false;
	}
	public void Move(Vector2 direction)
	{
		if (!EffectsManager.mutedEffects)
			source.PlayOneShot(onMoveClip);
		transform.DOMove((Vector2)transform.position + direction, GameSettings.tweenDuration);
	}
	protected AudioSource source;
	protected virtual void Awake()
	{
		source = GetComponent<AudioSource>();
	}

}