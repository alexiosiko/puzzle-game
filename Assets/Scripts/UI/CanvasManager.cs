using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
	[SerializeField] GameObject menuGameObject;
	[SerializeField] GameObject eventSystemPrefab;
	public void ChangeScene(string sceneName) => SceneLoader.LoadScene(sceneName);
	public void ToggleMenu()
	{
		if (menuGameObject.activeInHierarchy)
		{
			menuGameObject.SetActive(false);
		}
		else
		{
			menuGameObject.SetActive(true);
		}
	}
	public static CanvasManager Singleton;
	void Awake()
	{
		Singleton = this;
	}
}
