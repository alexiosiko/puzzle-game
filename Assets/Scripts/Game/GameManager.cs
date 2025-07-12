using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static void Restart()
	{
		DOTween.Clear();
		TurnManager.Singleton.StopAllCoroutines();
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
	}
	void HandleOnPlayerDie()
	{
		Restart();
	}
	void OnEnable()
	{
		Player.OnPlayerDie += HandleOnPlayerDie;
	}
	void OnDisable()
	{
		Player.OnPlayerDie -= HandleOnPlayerDie;
	}
}
