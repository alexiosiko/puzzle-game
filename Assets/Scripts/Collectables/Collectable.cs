using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Collectable : Moveable
{
	[SerializeField] AudioClip onPickupClip;
	SpriteRenderer _renderer; 
	Sprite firstSprite;
	public GameObject prefab;
	public string id;
	public Sprite sprite;
	public void Action(Player player)
	{
		TurnManager	.Singleton.AddInteractable(Pickup(player));
	}
	IEnumerator Pickup(Player player)
	{
		if (!EffectsManager.mutedEffects)
			source.PlayOneShot(onPickupClip);
		_renderer.sprite = firstSprite;
		player.inventory.AddCollectable(new CollectableData(id, _renderer, prefab));
		yield return transform.DOScaleX(0, GameSettings.tweenDuration).WaitForCompletion();
		Destroy(gameObject);
	}
	public void Break()
	{
		transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		_collider.enabled = false;
		Destroy(gameObject, GameSettings.tweenDuration);
	}
	BoxCollider2D _collider;
	protected override void Awake()
	{
		base.Awake();
		_collider = GetComponent<BoxCollider2D>();
		_renderer = GetComponent<SpriteRenderer>();
		firstSprite = _renderer.sprite;
	}
}
