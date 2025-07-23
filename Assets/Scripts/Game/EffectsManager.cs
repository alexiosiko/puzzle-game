using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EffectsManager : MonoBehaviour
{
	public static bool mutedEffects = false;
	public bool ToggleMute()
	{
		if (mutedEffects == true)
		{
			mutedEffects = false;
		}
		else
		{
			mutedEffects = true;
		}
		return mutedEffects;
	}
	public void PlayClip(AudioClip clip)
	{
		if (!mutedEffects)
			source.PlayOneShot(clip);
	}
	void Awake()
	{
		source = GetComponent<AudioSource>();
		Singleton = this;
	}
	AudioSource source;
	public static EffectsManager Singleton;
}
