using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Projectile : MonoBehaviour
{
	protected bool firstMove = true;
	[SerializeField] AudioClip explosionClip;
	public Vector2 direction;
	public int maxDistance;
	protected int currentDistance;
	[SerializeField] public LayerMask hitLayers;
	void OnEnable() => TurnManager.OnProjectilePhase += HandleOnProjectileTurn;
	void OnDisable() => TurnManager.OnProjectilePhase -= HandleOnProjectileTurn;
	public virtual void Init(Vector2 direction, int maxDistance)
	{
		this.direction = direction;
		this.maxDistance = maxDistance;
		transform.right = direction;
	}
	protected abstract void HandleOnProjectileTurn();
	public void Explode()
	{
		EffectsManager.Singleton.PlayClip(explosionClip);
		Invoke(nameof(LateDoKill), GameSettings.tweenDuration / 1.1f);
		Destroy(gameObject, GameSettings.tweenDuration);
	}
	void OnDestroy() => CancelInvoke();
	void LateDoKill() => transform.DOKill();
	protected bool CheckHit(Vector2 direction)
	{
		Vector2 newPos = (Vector2)transform.position + direction;
		var hit = Physics2D.OverlapPoint(newPos, hitLayers);
		print(newPos);
		if (hit)
		{
			if (hit.TryGetComponent(out Enemy e))
			{
				TurnManager.Singleton.AddDie(e.Die());
				return true;
			}
			if (hit.TryGetComponent(out Player p))
			{
				print("Added die");
				TurnManager.Singleton.AddDie(p.Die());
				return true;
			}
			return true;
		}
		return false;
	}
	protected abstract IEnumerator Move();
}
