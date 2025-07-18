using TMPro;
using UnityEngine;

public class MoveCount : MonoBehaviour
{

	TMP_Text text;
	void HandleOnPlayerPhase()
	{
		int current = int.Parse(text.text);
		current++;
		text.text = current.ToString();
	}
	void OnEnable() => TurnManager.OnPlayerPhase += HandleOnPlayerPhase;
	void OnDisable() => TurnManager.OnPlayerPhase -= HandleOnPlayerPhase;
	void Awake()
	{
		text = GetComponent<TMP_Text>();
		text.text = "0";
	}
}
