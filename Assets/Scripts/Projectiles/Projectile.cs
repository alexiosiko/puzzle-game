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
		Camera.main.transform.DOPunchRotation(new (0, 0, 2	), 0.2f);
		animator.Play("Explode");
		_collider.enabled = false;
		TurnManager.OnProjectilePhase -= HandleOnProjectileTurn;
		EffectsManager.Singleton.PlayClip(explosionClip);
		Invoke(nameof(LateDoKill), GameSettings.tweenDuration / 1.1f);
		Destroy(gameObject, 1f);
	}
	void OnDestroy() => CancelInvoke();
	void LateDoKill() => transform.DOKill();
	protected bool CheckHit(Vector2 direction)
	{
		Vector2 newPos = (Vector2)transform.position + direction;
		var hit = Physics2D.OverlapPoint(newPos, hitLayers);
		if (hit)
		{
			if (hit.TryGetComponent(out Enemy e))
			{
				TurnManager.Singleton.RemoveAttack(e.attackHashedCode);
				TurnManager.Singleton.AddDie(e.Die());
				return true;
			}
			if (hit.TryGetComponent(out Player p))
			{
				TurnManager.Singleton.AddDie(p.Die());
				return true;
			}
			return true;
		}
		return false;
	}
	Animator animator;
	BoxCollider2D _collider;
	void Awake()
	{
		_collider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
	}
	protected abstract IEnumerator Move();
}
