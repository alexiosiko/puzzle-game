using System.Collections;
using DG.Tweening;

public class Door : Interactable
{
	public string keyId;
	public override void Action(Player player)
	{
		var data = player.inventory.GetAndRemoveCollectableData(keyId);
		if (data == null)
			return;

		TurnManager.AddInteractable(Open());
	}

	public IEnumerator Open()
	{
		yield return transform.DOScale(2, 0.5f).WaitForCompletion();
		Destroy(gameObject);
	}
}
