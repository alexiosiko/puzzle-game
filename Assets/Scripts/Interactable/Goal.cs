using System.Collections;
using DG.Tweening;
using UnityEngine;
public class Goal : Moveable
{
	[SerializeField] AudioClip onInteractClip;
	public void Action(Player player)
	{
		// Do this to prevent infinite looping
		TurnManager.Singleton.enabled = false;
		
		source.PlayOneShot(onInteractClip);
		transform.DOScale(2, 0.5f).WaitForCompletion();
		Invoke(nameof(NextScene), 1f);
	}
	void NextScene() => SceneLoader.LoadScene();
	public IEnumerator Break()
	{
		yield return transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		GameManager.onGameLose?.Invoke();
	}
}
