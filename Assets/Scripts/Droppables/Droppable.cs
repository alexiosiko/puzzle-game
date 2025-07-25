using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Droppable : MonoBehaviour
{
	[SerializeField] AudioClip dropClip;
	AudioSource source;
	protected virtual void Awake()
	{
		source = GetComponent<AudioSource>();
	}
	protected virtual void Start()
	{
		if (!EffectsManager.mutedEffects)
			source.PlayOneShot(dropClip);
	}
}
