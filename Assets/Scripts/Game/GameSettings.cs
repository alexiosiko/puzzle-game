using UnityEngine;

public static class GameSettings
{
	public static Vector2 rayCastOffset = new(0.1f, -0.2f);
	public static float tweenDuration = 0.2f;
	public static int notWalkableLayers = LayerMask.GetMask("Wall", "Moveable", "Breakable");

}
