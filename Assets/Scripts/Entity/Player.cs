using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player : Entity
{
	[SerializeField] AudioClip[] dieClips;
	[SerializeField] AudioClip[] cheerClips;
	[SerializeField] AudioClip dropClip;
	protected override IEnumerator Move(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Collectable"));
		if (hit)
			hit.GetComponent<Collectable>()?.Action(this);
		HitProjectile(pos);

		PlayClip(footstepClips);
		FaceEntity(pos);
		transform.DOMove(pos, GameSettings.tweenDuration);
		yield return new WaitForSeconds(GameSettings.tweenDuration / 1.7f);
		
	}

	void TryDropBomb()
	{
		var c = inventory.GetCollectableData("bomb");
		if (c == null)
			return;
		var hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Droppable"));
		if (hit)
			return;

		
		Instantiate(c.prefab, transform.position, Quaternion.identity);
		inventory.RemoveCollectable(c);
	}
	bool CanMove(Vector2 pos, Vector2 direction)
	{
		if (direction == Vector2.zero || (Vector2)transform.position == pos)
			return false;
		pos = pos + GameSettings.rayCastOffset;

		// LayerMask exclude = ~LayerMask.GetMask("Collectable", "Droppable");
		var hit = Physics2D.OverlapPoint(pos, notWalkableLayers);
		if (hit)
		{
			if (hit.TryGetComponent(out Moveable m))
					return m.CanMove(direction);
			hit.GetComponent<Interactable>()?.Action(this);
			return false;
		}
		return true;
	}
	public override IEnumerator Die()
	{
		// yield return base.Die();
		animator.Play("Die");
		PlayClip(dieClips);
		yield return new WaitForSeconds(1f);
		OnPlayerDie?.Invoke();
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

			newPos = (Vector2)transform.position + dir;

			// IF goal
			Goal g = HitGoal(newPos);
			if (g)
			{
				g.Action(this);
				yield break;
			}

			canMove = CanMove(newPos, dir);

			if (!canMove)
			{
				// If cannot move, try interact
				yield return Interact(newPos);
				dir = Vector2.zero;
			}

		} while (!canMove);


		// Handle input convienience
		dir = Vector2.zero;
		checkingForInput = false;
		float fingerPressTime = 0.175f;
		Invoke(nameof(EnableSetCheckingForInput), fingerPressTime);


		yield return Move(newPos);
	}
	Goal HitGoal(Vector2 pos)
	{
		var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Goal"));
		if (hit && hit.TryGetComponent(out Goal g))
			return g;
		return null;	
	}
	IEnumerator Interact(Vector2 pos)
	{
		// Collectable just to include goal
		int layerMask = LayerMask.GetMask("Interactable");
		var hit = Physics2D.OverlapPoint(pos, layerMask);
		if (hit)
		{
			if (hit.TryGetComponent(out Goal g))
				yield break;
			else
				yield return hit.GetComponent<Interactable>().Action(this);
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


		
		     if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) dir = Vector2.up;
		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) dir = Vector2.down;
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) dir = Vector2.left;
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) dir = Vector2.right;
}
	void OnEnable()
	{
		TurnManager.OnPlayerPhase += HandleOnPlayerPhase;
		Goal.OnGameWin += HandleOnGameWin;
	}
	void OnDisable()
	{
		TurnManager.OnPlayerPhase -= HandleOnPlayerPhase;
		Goal.OnGameWin -= HandleOnGameWin;
	}
	void HandleOnGameWin()
	{
		PlayClip(cheerClips);	
		animator.Play("Cheer");
		enabled = false;
	}
	void HandleOnPlayerPhase() => TurnManager.Singleton.AddPlayer(WaitForInput());
	[HideInInspector] public Inventory inventory;
	protected override void Awake()
	{
		base.Awake();
		inventory = GetComponent<Inventory>();
	}


}