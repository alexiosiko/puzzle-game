
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
public class PopupMessageManager : MonoBehaviour
{
	float textSpeed = 0.045f;
	public void PopupMessage(string message)
	{
		if (message.Length == 0)
			return;
		StopAllCoroutines();
		StartCoroutine(IPopupMessage(message));  
	}
	IEnumerator IPopupMessage(string message)
	{
		transform.DOKill();
		yield return AnimateText(message);
		yield return new WaitForSeconds(2f);
		yield return AnimateRemoveText();
	}

	IEnumerator AnimateRemoveText()
	{
		while (text.text.Length > 0)
		{
			string text = this.text.text;
			text = text.Remove(text.Length - 1);
			this.text.text = text;
			yield return new WaitForSeconds(textSpeed);
		}
	}
	IEnumerator AnimateText(string message)
	{
		text.text = "";
		for (int i = 0; i < message.Length; i++)
		{
			text.text += message[i];
			yield return new WaitForSeconds(textSpeed);
		}
	}
	string originalText;
	public void Start()
	{
		originalText = text.text;
		text.text = "";
		Invoke(nameof(DelayedStart), 1f);
	}
	void DelayedStart() => PopupMessage(originalText);
	TMP_Text text;
	void Awake()
	{
		text = GetComponent<TMP_Text>();		
	}
}
