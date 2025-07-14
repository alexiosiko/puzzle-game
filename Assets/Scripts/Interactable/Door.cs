using System.Collections;
using UnityEngine;
public class Door : Interactable
{
	[SerializeField] AudioClip doorRattleClip;
	Animator animator;
	public string keyId;
	public override IEnumerator Action(Player player)
	{
		var data = player.inventory.GetAndRemoveCollectableData(keyId);
		if (data == null)
			yield return Rattle();
		else
			yield return Open();
	}
	IEnumerator Rattle()
	{
		source.PlayOneShot(doorRattleClip);
		yield return new WaitForSeconds(GameSettings.tweenDuration);
	}

	IEnumerator Open()
	{
		source.PlayOneShot(onInteractClip);
		yield return new WaitForSeconds(GameSettings.tweenDuration);
		Destroy(gameObject);
	}

	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}
}
