using UnityEngine;

public static class Utils
{
	public static Vector2Int[] directions = {
			Vector2Int.left,
			Vector2Int.right,
			Vector2Int.up,
			Vector2Int.down,
		};
	public static Vector2Int[] GetRandomDirections()
	{
		for (int i = 0; i < directions.Length - 1; i++)
		{
			int j = Random.Range(0, i + 1);
			(directions[i], directions[j]) = (directions[j], directions[i]);
		}

		return directions;
	}
	public static AudioClip GetRandomAudioClip(AudioClip[] clips)
	{
		if (clips.Length == 0)
			return null;
		return clips[Random.Range(0, clips.Length)];
	}
}
