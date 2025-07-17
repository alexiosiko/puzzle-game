using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static Action onGameLose;
	public static void Restart()
	{
		onGameLose?.Invoke();
		DOTween.Clear(); // CLear tweens just incase
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	void OnEnable()
	{
		Player.OnPlayerDie += Restart;
	}
	void OnDisable()
	{
		Player.OnPlayerDie -= Restart;
	}
	void OnDestroy()
	{
		DOTween.KillAll();
	}

}
