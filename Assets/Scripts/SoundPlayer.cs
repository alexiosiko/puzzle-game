using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
	protected void PlayClip(AudioClip clip)
	{
		if (!EffectsManager.mutedEffects)
			source.PlayOneShot(clip);
	}
	protected void PlayClip(AudioClip[] clips)
	{
		var clip = Utils.GetRandomAudioClip(clips);
		if (clip)
		{
			if (!EffectsManager.mutedEffects)
				source.PlayOneShot(clip);
		}
	}
	AudioSource source;
	protected virtual void Awake() => source = GetComponent<AudioSource>();
}
