using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public void LoadScene(string sceneName) => SceneLoader.NextScene(sceneName);
}
