using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

	protected void PlayAudio(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}
	protected void PlayAudio(AudioClip[] clips)
	{
		var clip = Utils.GetRandomAudioClip(clips);
		if (clip)
			source.PlayOneShot(clip);
	}
	AudioSource source;
	protected virtual void Awake() => source = GetComponent<AudioSource>();
}
