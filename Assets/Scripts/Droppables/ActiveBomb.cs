using System.Collections;
using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class ActiveBomb : Droppable
{
	[SerializeField] GameObject explosionGameObject;
	[SerializeField] Sprite[] sprites = new Sprite[2];
	int currentStep = 0;
	public void OnEnable() => TurnManager.OnBombPhase += HandleOnBombPhase;
	void OnDisable() => TurnManager.OnBombPhase -= HandleOnBombPhase;
	void HandleOnBombPhase()
	{
		if (currentStep == 2)
		{
			Camera.main.transform.DOPunchRotation(new (0, 0, 2	), 0.2f);
			TurnManager.Singleton.AddBomb(ExplodeRoutine());
			return;
		}
		_renderer.sprite = sprites[currentStep];
		currentStep++;

	}
	IEnumerator ExplodeRoutine()
	{
		// spawn explosions
		ExplodeInDirection(Vector2.left);
		ExplodeInDirection(Vector2.up);
		ExplodeInDirection(Vector2.right);
		ExplodeInDirection(Vector2.down);
		var b = SpawnExplosion((Vector2)transform.position);
		b.GetComponent<Explosion>().PlayExplosionSound();

		Destroy(gameObject);

		yield break;

	}
	void ExplodeInDirection(Vector2 direction)
	{
		int stopExplosionLayer = LayerMask.GetMask("Wall", "Moveable");
		for (int i = 1; i < 3; i++)
		{
			Vector2 pos = (Vector2)transform.position + direction * i;
			var hit = Physics2D.OverlapPoint(pos, stopExplosionLayer);
			if (hit)
				return;
			SpawnExplosion(pos);
		}

	}
	protected override void Awake()
	{
		base.Awake();
		_renderer = GetComponent<SpriteRenderer>();
	}
	protected SpriteRenderer _renderer;
	GameObject SpawnExplosion(Vector2 pos) => Instantiate(explosionGameObject, pos, Quaternion.identity);
}