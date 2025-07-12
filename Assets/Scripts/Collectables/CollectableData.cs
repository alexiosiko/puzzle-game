using UnityEngine;

[CreateAssetMenu(menuName = "Collectable/Data")]
public class CollectableData : ScriptableObject
{
	public GameObject prefab;
	public RuntimeAnimatorController controller;
	public string id;
	public AudioClip onPickupAudioClip;
	public Sprite sprite;
}
