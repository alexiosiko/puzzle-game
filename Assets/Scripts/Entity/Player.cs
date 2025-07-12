using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player : Entity
{
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);
		var hit = Physics2D.Raycast(transform.position, Vector2.zero, 10f, LayerMask.GetMask("Collectable"));
		if (hit.collider)
			hit.collider.GetComponent<Interactable>()?.Action(this);
	}

	void TryDropBomb()
	{
		var c = inventory.GetCollectableData("bomb");
		if (c == null)
			return;

		var b = Instantiate(c.prefab, transform.position, Quaternion.identity);
		inventory.RemoveCollectable(c);
	}
	bool CanMove(Vector2 pos, Vector2 direction)
	{
		LayerMask exclude = ~LayerMask.GetMask("Collectable");
		var hit = Physics2D.Raycast(pos, Vector2.zero, 10f, exclude);
		if (hit.collider)
		{
			if (hit.collider.TryGetComponent<Goal>(out Goal g))
			{
				g.Action(this);
				return true;
			}
			// else if (hit.collider.TryGetComponent<Enemy>(out Enemy e))
			// {
			// 	TurnManager.Singleton.StopAllCoroutines();
			// 	StartCoroutine(e.Action(this));
			// 	return false;
			// }
			else if (hit.collider.TryGetComponent<Door>(out Door d))
			{
				d.Action(this);
				return false;
			}
			else if (hit.collider.TryGetComponent<Moveable>(out Moveable m))
				return m.CanMove(direction);
			
			return false;
		}
		return true;
	}
	public override IEnumerator Die()
	{
		yield return transform.DOScale(0, 0.5f).WaitForCompletion();
		OnPlayerDie?.Invoke();
	}
	public static Action OnPlayerDie;
	IEnumerator WaitForInput()
	{
		Vector2 newPos;
		Vector2 dir;
		do
		{
			dir = Vector2.zero;
			while (dir == Vector2.zero)
			{
				if (Input.GetKeyDown(KeyCode.Space))
					TryDropBomb();

				if (Input.GetKey(KeyCode.W)) dir = Vector2.up;
				else if (Input.GetKey(KeyCode.S)) dir = Vector2.down;
				else if (Input.GetKey(KeyCode.A)) dir = Vector2.left;
				else if (Input.GetKey(KeyCode.D)) dir = Vector2.right;

				yield return null;
			}

			newPos = (Vector2)transform.position + dir;
		} while (CanMove(newPos, dir) == false);
		yield return Move(newPos);
	}
	void OnEnable() => TurnManager.OnPlayerPhase += HandleOnPlayerPhase;
	void OnDisable() => TurnManager.OnPlayerPhase -= HandleOnPlayerPhase;
	void HandleOnPlayerPhase() => TurnManager.AddPlayer(WaitForInput());
	public Inventory inventory;
	protected override void Awake()
	{
		base.Awake();
		inventory = GetComponent<Inventory>();
	}


}
