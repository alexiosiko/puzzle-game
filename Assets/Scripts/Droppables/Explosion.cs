using System.Runtime.InteropServices;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
	[SerializeField] AudioClip explosionClip;
	AudioSource source;
	void Awake() => source = GetComponent<AudioSource>();
	public void PlayExplosionSound() => source.PlayOneShot(explosionClip);
	void Start()
	{
		var hits = Physics2D.OverlapPointAll(transform.position, LayerMask.GetMask("Breakable", "Entity", "Collectable"));
		foreach(var hit in hits)
		{
			if (hit.TryGetComponent(out Goal g))
				TurnManager.Singleton.AddExplosion(g.Break());
			else if (hit.TryGetComponent(out Collectable c))
				TurnManager.Singleton.AddExplosion(c.Break());

			else if (hit.TryGetComponent(out Breakable b))
				TurnManager.Singleton.AddExplosion(b.Break());
			else if (hit.TryGetComponent(out Player p))
				TurnManager.Singleton.AddExplosion(p.Die());
			else if (hit.TryGetComponent(out Enemy e))
			{
				TurnManager.Singleton.AddExplosion(e.Die());
				TurnManager.Singleton.RemoveAttack(e.attackHashedCode);

			}
		}

		Destroy(gameObject, 0.2f);
    }
}
