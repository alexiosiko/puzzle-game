using UnityEngine;

public static class GameSettings
{
	public static float tweenDuration = 0.2f;
	public static int notWalkableLayers = LayerMask.GetMask("Wall", "Moveable", "Breakable", "Entity");

}
