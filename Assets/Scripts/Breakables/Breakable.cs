using System.Collections;
using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Breakable : MonoBehaviour
{
	public IEnumerator Break()
	{
		yield return transform.DOScaleX(0, 0.2f).WaitForCompletion();
		Destroy(gameObject);
	}
}
