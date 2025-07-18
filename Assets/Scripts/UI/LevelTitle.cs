using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelTitle : MonoBehaviour
{
	TMP_Text text;
	void Awake()
	{
		text = GetComponent<TMP_Text>();
		text.text = "Level " + SceneManager.GetActiveScene().name;
	}
}
