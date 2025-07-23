using System.Security.Authentication;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	[SerializeField] AudioClip[] musicClips;
	AudioSource source;
	public static MusicManager Singleton;
	void Awake()
	{
		if (Singleton != null)
		{
			Destroy(gameObject);
			return;
		}
		Singleton = this;

		source = GetComponent<AudioSource>();
		DontDestroyOnLoad(gameObject);
		PlayNextSong();
	}
	bool muted;
	public bool ToggleMute()
	{
		if (muted)
		{
			muted = false;
			source.volume = 0.5f;
		}
		else
		{
			muted = true;
			source.volume = 0;
		}
		return muted;
	}
	void PlayNextSong()
	{
		if (musicClips.Length == 0)
			return;
		if (musicClips.Length == 1)
		{
			if (!EffectsManager.mutedEffects)
				source.PlayOneShot(musicClips[0]);
			return;
		}

		AudioClip next;
		do
		{
			next = musicClips[Random.Range(0, musicClips.Length)];
		} while (next == source.clip);
		source.clip = next;
		source.Play();
		Invoke(nameof(PlayNextSong), source.clip.length);
	}

}
