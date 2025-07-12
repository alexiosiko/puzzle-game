using System.Collections;
using DG.Tweening;
public class Goal : Interactable
{
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
}
