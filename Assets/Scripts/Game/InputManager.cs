using UnityEngine;

public class InputManager : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
			GameManager.Restart();

		if (Input.GetKeyDown(KeyCode.K))
			SceneLoader.NextScene();

		if (Input.GetKeyDown(KeyCode.Escape))
			CanvasManager.Singleton.ToggleMenu();
		
    }
}
