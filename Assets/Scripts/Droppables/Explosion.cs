using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
	void Start()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 10f, LayerMask.GetMask("Breakable", "Entity", "Collectable"));
		foreach(var hit in hits)
		{
			if (hit.collider.TryGetComponent(out Goal g))
			{
				TurnManager.AddExplosion(g.Break());
			}
			if (hit.collider.TryGetComponent(out Collectable c))
					TurnManager.AddExplosion(c.Break());

			if (hit.collider.TryGetComponent(out Breakable b))
				TurnManager.AddExplosion(b.Break());

			if (hit.collider.TryGetComponent(out Entity e))
				TurnManager.AddExplosion(e.Die());
		}
		// TurnManager.Singleton.Wait(0.2f);

		Destroy(gameObject, 0.2f);
    }
}
