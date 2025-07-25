using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EffectsManager : MonoBehaviour
{
	public static Action OnShake;
	[HideInInspector] public Transform cameraTransform;
	public void ResetAndCallOnShake()
	{
		OnShake?.Invoke();
		cameraTransform.DOKill();
		cameraTransform.localScale = Vector3.one;
		cameraTransform.rotation = Quaternion.Euler(0, 0, 0);
	}
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
		cameraTransform = Camera.main.transform;
	}
	AudioSource source;
	public static EffectsManager Singleton;
}
