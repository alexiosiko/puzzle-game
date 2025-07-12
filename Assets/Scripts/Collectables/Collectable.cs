using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Collectable : Interactable
{
	[SerializeField] CollectableData data;
	Animator animator;
	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
		animator.runtimeAnimatorController = data.controller; ;
	}
	public override void Action(Player player)
	{
		TurnManager.AddInteractable(Pickup(player));

	}
	IEnumerator Pickup(Player player)
	{
		source.PlayOneShot(data.onPickupAudioClip);
		player.inventory.AddCollectable(data);
		yield return transform.DOScaleX(0, 0.25f).WaitForCompletion();
		Destroy(gameObject);
	}

}
