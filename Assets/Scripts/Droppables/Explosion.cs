using System.Runtime.InteropServices;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
	[SerializeField] AudioClip explosionClip;
	AudioSource source;
	void Awake() => source = GetComponent<AudioSource>();
	public void PlayExplosionSound()
	{
		if (!EffectsManager.mutedEffects)
			source.PlayOneShot(explosionClip);
	} 
	void Start()
	{
		var hits = Physics2D.OverlapPointAll(transform.position, LayerMask.GetMask("Breakable", "Enemy", "Collectable", "Player"));
		foreach(var hit in hits)
		{
			if (hit.TryGetComponent(out Goal g))
				g.Break();
			else if (hit.TryGetComponent(out Collectable c))
				c.Break();

			else if (hit.TryGetComponent(out Breakable b))
				b.Break();
			else if (hit.TryGetComponent(out Player p))
				TurnManager.Singleton.AddExplosion(p.Die());
			else if (hit.TryGetComponent(out Enemy e))
			{
				TurnManager.Singleton.RemoveAttack(e.attackHashedCode);
				TurnManager.Singleton.AddExplosion(e.Die());

			}
		}

		Destroy(gameObject, 0.2f);
    }
}
