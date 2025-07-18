using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
	public static void NextScene()
	{
		string currentSceneName = SceneManager.GetActiveScene().name;
		int num = int.Parse(currentSceneName);
		string newSceneName = (num + 1).ToString();
		NextScene(newSceneName);
	}

	public static void NextScene(string sceneName)
	{
		TurnManager.Singleton.enabled = false;
		SceneManager.LoadScene(sceneName);
	}
	
}
