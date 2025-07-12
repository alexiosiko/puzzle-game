using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Moveable : MonoBehaviour
{
	public bool CanMove(Vector2 direction)
	{
		Vector2 targetPos = (Vector2)transform.position + direction;

		// Ignore self and check target
		RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero, 1f);

		if (!hit)
		{
			Move(direction);
			return true;
		}

		// If another moveable, check recursively
		if (hit.collider.TryGetComponent<Moveable>(out Moveable m))
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
		source.Play();
		transform.DOMove((Vector2)transform.position + direction, GameSettings.tweenDuration);
	}
	AudioSource source;
	void Awake()
	{
		source = GetComponent<AudioSource>();
	}
}
