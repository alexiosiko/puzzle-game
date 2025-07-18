using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	[SerializeField] GameObject menuGameObject;
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
