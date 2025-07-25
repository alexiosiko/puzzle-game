using System.Collections;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Breakable : SoundPlayer
{
	[SerializeField] AudioClip explodeClip;
	public void Break()
	{
		PlayClip(explodeClip);
		animator.Play("Explode");
		enabled = false;
		_collider.enabled = false;
		Destroy(gameObject, 0.5f);
	}
	Animator animator;
	BoxCollider2D _collider;
	protected override void Awake()
	{
		base.Awake();
		_collider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
	}
	void OnEnable() => EffectsManager.OnShake += HandleOnShake;
	void OnDisable() => EffectsManager.OnShake -= HandleOnShake;
	void HandleOnShake()
	{
		animator.Play("Shake");
	}
}
