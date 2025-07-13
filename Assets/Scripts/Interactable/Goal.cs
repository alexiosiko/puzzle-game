using System.Collections;
using DG.Tweening;
using UnityEngine;
public class Goal : Interactable
{
	[SerializeField] AudioClip breakClip;
	public override void Action(Player player)
	{
		source.Play();
		TurnManager.Singleton.StopAllCoroutines();
		StartCoroutine(LoadNextScene());
	}
	IEnumerator LoadNextScene()
	{
		yield return transform.DOScale(2, 0.5f).WaitForCompletion();
		SceneLoader.NextScene();
	}
	public IEnumerator Break()
	{
		source.PlayOneShot(breakClip);
		yield return transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		GameManager.onGameLose?.Invoke();
	}
}
