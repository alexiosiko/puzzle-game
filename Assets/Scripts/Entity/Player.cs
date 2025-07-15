using System;
using System.Collections;
using System.Threading;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player : Entity
{
	[SerializeField] AudioClip dropClip;
	protected override IEnumerator Move(Vector2 pos)
	{
		yield return base.Move(pos);  
		var hit = Physics2D.Raycast(pos, Vector2.zero, 10f, LayerMask.GetMask("Collectable"));
		if (hit.collider)
			hit.collider.GetComponent<Collectable>()?.Action(this);
	}

	void TryDropBomb()
	{
		var c = inventory.GetCollectableData("bomb");
		if (c == null)
			return;
		
		Instantiate(c.prefab, transform.position, Quaternion.identity);
		inventory.RemoveCollectable(c);
	}
	bool CanMove(Vector2 pos, Vector2 direction)
	{
		if (direction == Vector2.zero || (Vector2)transform.position == pos)
			return false;

		LayerMask exclude = ~LayerMask.GetMask("Collectable");
		var hit = Physics2D.Raycast(pos + GameSettings.rayCastOffset, Vector2.zero, 10f, exclude);
		if (hit.collider)
		{
			if (hit.collider.TryGetComponent(out Moveable m))
				return m.CanMove(direction);
			hit.collider.GetComponent<Interactable>()?.Action(this);
			return false;
		}
		return true;
	}
	public override IEnumerator Die()
	{
		yield return transform.DOScale(0, 0.5f).WaitForCompletion();
		GameManager.Restart();
	}
	public static Action OnPlayerDie;
	void OnDestroy() => OnPlayerDie = null;
	Vector2 dir;
	IEnumerator WaitForInput()
	{
		Vector2 newPos = Vector2.zero;
		bool canMove = false;

		do
		{
			while (dir == Vector2.zero)
			{
				if (wantsToDropBomb)
				{
					wantsToDropBomb = false;
					TryDropBomb();
				}

				yield return null;
			}

			newPos = (Vector2)transform.position + dir;
			yield return HitGoal(newPos);

			canMove = CanMove(newPos, dir);

			if (!canMove)
			{
				// If cannot move, try interact
				yield return Interact(newPos);
				dir = Vector2.zero;
			}

		} while (!canMove);

		// Extra
		dir = Vector2.zero;
		float fingerPressTime = 0.2f;
		Invoke(nameof(EnableSetCheckingForInput), fingerPressTime);

		checkingForInput = false;
		yield return Move(newPos);
	}
	IEnumerator HitGoal(Vector2 pos)
	{
		var hit = Physics2D.Raycast(pos, Vector2.zero, 1f, LayerMask.GetMask("Goal"));
		if (hit && hit.collider.TryGetComponent(out Goal g))
			yield return g.Action(this);
	}
	IEnumerator Interact(Vector2 pos)
	{
		// Collectable just to include goal
		int layerMask = LayerMask.GetMask("Interactable");
		var hit = Physics2D.Raycast(pos, Vector2.zero, 10f, layerMask);
		if (hit.collider)
		{
			if (hit.collider.TryGetComponent(out Goal g))
				yield return g.Action(this);
			else
				yield return hit.collider.GetComponent<Interactable>().Action(this);
		}
	}
	bool wantsToDropBomb = false;
	public bool checkingForInput = true;
	void EnableSetCheckingForInput() => checkingForInput = true;
	void Update()
	{
		if (checkingForInput == false)
			return;
			
		if (Input.GetKeyDown(KeyCode.Space))
			wantsToDropBomb = true;

		
		if (Input.GetKey(KeyCode.W)) dir = Vector2.up;
		else if (Input.GetKey(KeyCode.S)) dir = Vector2.down;
		else if (Input.GetKey(KeyCode.A)) dir = Vector2.left;
		else if (Input.GetKey(KeyCode.D)) dir = Vector2.right;
	}
	void OnEnable() => TurnManager.OnPlayerPhase += HandleOnPlayerPhase;
	void OnDisable() => TurnManager.OnPlayerPhase -= HandleOnPlayerPhase;
	void HandleOnPlayerPhase() => TurnManager.Singleton.AddPlayer(WaitForInput());
	[HideInInspector] public Inventory inventory;
	protected override void Awake()
	{
		base.Awake();
		inventory = GetComponent<Inventory>();
	}


}