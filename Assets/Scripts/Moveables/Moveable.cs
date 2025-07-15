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
		RaycastHit2D hit = Physics2D.Raycast(targetPos + GameSettings.rayCastOffset, Vector2.zero, 1f);

		if (!hit)
		{
			Move(direction);
			return true;
		}

		// If another moveable, check recursively
		if (hit.collider.TryGetComponent(out Moveable m))
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
		source.PlayOneShot(onMoveClip);
		transform.DOMove((Vector2)transform.position + direction, GameSettings.tweenDuration);
	}
	protected AudioSource source;
	protected virtual void Awake()
	{
		source = GetComponent<AudioSource>();
	}

}