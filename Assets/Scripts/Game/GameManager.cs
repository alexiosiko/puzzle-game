using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static Action onGameLose;
	public static void Restart()
	{
		onGameLose?.Invoke();
		DOTween.Clear();
		TurnManager.Singleton.StopAllCoroutines();
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
	}

}
