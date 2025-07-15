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
			yield return Open(true);
	}
	IEnumerator Rattle()
	{
		source.PlayOneShot(doorRattleClip);
		yield return new WaitForSeconds(GameSettings.tweenDuration);
	}

	IEnumerator Open(bool checkForNearby)
	{
		source.PlayOneShot(onInteractClip);
		if (checkForNearby)
			OpenNearBy();
		yield return new WaitForSeconds(GameSettings.tweenDuration);
		Destroy(gameObject);
	}
	void OpenNearBy()
	{
		foreach (var dir in Utils.directions)
		{
			Vector2 pos = (Vector2)transform.position + dir;
			var hit = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Interactable"));
			if (hit && hit.TryGetComponent(out Door d))
				d.StartCoroutine(d.Open(false));
		}
	}

	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}
}
