using System.Collections;
using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class ActiveBomb : Droppable
{
	[SerializeField] GameObject explosionGameObject;
	[SerializeField] Sprite[] sprites = new Sprite[2];
	int currentStep = 0;
	void OnEnable() => TurnManager.OnBombPhase += HandleOnBombPhase;
	void OnDisable() => TurnManager.OnBombPhase -= HandleOnBombPhase;
	void HandleOnBombPhase()
	{
		if (currentStep == 2)
		{
			EffectsManager.Singleton.ResetAndCallOnShake();
			EffectsManager.Singleton.cameraTransform.DOPunchRotation(new (0, 0, 5), 0.15f);
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
		var b = SpawnExplosion(transform.position);
		b.GetComponent<Explosion>().PlayExplosionSound();

		Destroy(gameObject);

		yield break;

	}
	[SerializeField] LayerMask stopExplosionLayer;
	void ExplodeInDirection(Vector2 direction)
	{
		
		for (int i = 1; i < 3; i++)
		{
			Vector2 pos = (Vector2)transform.position + direction * i;
			var hit = Physics2D.OverlapPoint(pos + GameSettings.rayCastOffset, stopExplosionLayer);
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