using UnityEngine;

public static class Utils
{
	public static Vector2[] directions = {
			Vector2.left,
			Vector2.right,
			Vector2.up,
			Vector2.down,
		};
	public static Vector2[] GetRandomDirections()
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
