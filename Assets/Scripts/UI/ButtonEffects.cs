
using UnityEngine;
public class ButtonEffects : MonoBehaviour
{
	[SerializeField] AudioClip hoverClip;
	[SerializeField] AudioClip clickClip;
	public void PlayHoverClip() => EffectsManager.Singleton.PlayClip(hoverClip);
	public void PlayClickClip() => EffectsManager.Singleton.PlayClip(clickClip);

}
