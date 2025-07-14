using System.Collections;
using DG.Tweening;
using UnityEngine;
public class Goal : Moveable
{
	[SerializeField] AudioClip onInteractClip;
	public IEnumerator Action(Player player)
	{
		source.PlayOneShot(onInteractClip);
		yield return transform.DOScale(2, 0.5f).WaitForCompletion();
		SceneLoader.NextScene();
	}
	public IEnumerator Break()
	{
		yield return transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		GameManager.onGameLose?.Invoke();
	}
}
