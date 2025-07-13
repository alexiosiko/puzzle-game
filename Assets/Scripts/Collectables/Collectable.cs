using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Collectable : Moveable
{
	new SpriteRenderer renderer;
	Sprite firstSprite;
	public GameObject prefab;
	public string id;
	public AudioClip onPickupAudioClip;
	public Sprite sprite;
	public override void Action(Player player)
	{
		TurnManager.AddInteractable(Pickup(player));
	}
	IEnumerator Pickup(Player player)
	{
		source.PlayOneShot(onPickupAudioClip);
		renderer.sprite = firstSprite;
		player.inventory.AddCollectable(new CollectableData(id, renderer, prefab));
		yield return transform.DOScaleX(0, GameSettings.tweenDuration).WaitForCompletion();
		Destroy(gameObject);
	}
	public IEnumerator Break()
	{
		yield return transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		Destroy(gameObject);
	}

	protected override void Awake()
	{
		base.Awake();
		renderer = GetComponent<SpriteRenderer>();
		firstSprite = renderer.sprite;
	}
}
