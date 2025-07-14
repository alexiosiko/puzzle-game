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
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 10f, LayerMask.GetMask("Breakable", "Entity", "Collectable"));
		foreach(var hit in hits)
		{
			if (hit.collider.TryGetComponent(out Goal g))
			{
				TurnManager.Singleton.AddExplosion(g.Break());
			}
			if (hit.collider.TryGetComponent(out Collectable c))
					TurnManager.Singleton.AddExplosion(c.Break());

			if (hit.collider.TryGetComponent(out Breakable b))
				TurnManager.Singleton.AddExplosion(b.Break());

			if (hit.collider.TryGetComponent(out Entity e))
				TurnManager.Singleton.AddExplosion(e.Die());
		}

		Destroy(gameObject, 0.2f);
    }
}
