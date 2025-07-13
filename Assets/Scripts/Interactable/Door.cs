using System.Collections;
using UnityEngine;
public class Door : Interactable
{
	[SerializeField] AudioClip doorRattleClip;
	Animator animator;
	public string keyId;
	public override void Action(Player player)
	{
		var data = player.inventory.GetAndRemoveCollectableData(keyId);
		TurnManager.Singleton.StopAllCoroutines();
		if (data == null)
			StartCoroutine(Rattle());
		else
			StartCoroutine(Open());
	}
	IEnumerator Rattle()
	{
		source.PlayOneShot(doorRattleClip);
		yield return new WaitForSeconds(GameSettings.tweenDuration);
		TurnManager.Singleton.Start();
	}

	IEnumerator Open()
	{
		source.PlayOneShot(onInteractClip);
		yield return new WaitForSeconds(GameSettings.tweenDuration);
		TurnManager.Singleton.Start();
		Destroy(gameObject);
	}

	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}
}
