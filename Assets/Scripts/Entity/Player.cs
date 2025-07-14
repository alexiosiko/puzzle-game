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
		var hit = Physics2D.Raycast((Vector2)transform.position + GameSettings.rayCastOffset, Vector2.zero, 10f, LayerMask.GetMask("Collectable"));
		if (hit.collider)
			hit.collider.GetComponent<Interactable>()?.Action(this);
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
			if (hit.collider.TryGetComponent(out Door d))
			{
				d.Action(this);
				return false;
			}
		
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

			// Still good idea to check again before raycast

			newPos = (Vector2)transform.position + dir;
			canMove = CanMove(newPos, dir);
			if (!canMove)
				dir = Vector2.zero;
		} while (!canMove);
		dir = Vector2.zero;

		float fingerPressTime = 0.2f;
		Invoke(nameof(EnableSetCheckingForInput), fingerPressTime);

		checkingForInput = false;
		yield return Move(newPos);
	}
	bool wantsToDropBomb = false;
	bool checkingForInput = true;
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
	void HandleOnPlayerPhase() => TurnManager.AddPlayer(WaitForInput());
	public Inventory inventory;
	protected override void Awake()
	{
		base.Awake();
		inventory = GetComponent<Inventory>();
	}


}