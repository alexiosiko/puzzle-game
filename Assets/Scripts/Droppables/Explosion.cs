using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
	void Start()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 10f, LayerMask.GetMask("Breakable", "Entity"));
		foreach(var hit in hits)
		{
			if (hit.collider.TryGetComponent(out Breakable b))
				TurnManager.AddKillOrBreak(b.Break());

			else if (hit.collider.TryGetComponent(out Entity e))
				TurnManager.AddKillOrBreak(e.Die());
		}
		// TurnManager.Singleton.Wait(0.2f);

		Destroy(gameObject, 0.2f);
    }
}
