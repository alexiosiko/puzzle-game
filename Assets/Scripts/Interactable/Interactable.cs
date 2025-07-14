using System.Collections;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class Interactable : MonoBehaviour
{
	[SerializeField] protected AudioClip onInteractClip;
	protected AudioSource source;
	protected virtual void Awake() => source = GetComponent<AudioSource>();
	public abstract IEnumerator Action(Player player);
}
