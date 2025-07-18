using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	// [SerializeField] AudioClip gameLoseClip;
	// void HandleOnGameLose() => source.PlayOneShot(gameLoseClip);
	public void PlayClip(AudioClip clip) => source.PlayOneShot(clip);
	void Awake()
	{
		source = GetComponent<AudioSource>();
		Singleton = this;
	}
	AudioSource source;
	public static AudioManager Singleton;
}
