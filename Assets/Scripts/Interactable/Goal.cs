using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
public class Goal : Moveable
{
	public static Action OnGameWin;
	[SerializeField] AudioClip onInteractClip;
	public void Action(Player player)
	{
		// Do this to prevent infinite looping
		OnGameWin?.Invoke();

		if (!EffectsManager.mutedEffects)
			source.PlayOneShot(onInteractClip);
		// transform.DOScale(2, 0.5f).WaitForCompletion();
		Invoke(nameof(NextScene), 2f);
	}
	void NextScene() => SceneLoader.NextScene();
	public void Break()
	{
		transform.DOScale(0, GameSettings.tweenDuration).WaitForCompletion();
		GameManager.onGameLose?.Invoke();
	}
	protected override void Awake()
	{
		base.Awake();
	}
}
