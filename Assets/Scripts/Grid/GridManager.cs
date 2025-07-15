// using UnityEngine;
// using UnityEngine.Tilemaps;

// public class GridManager : MonoBehaviour
// {
//     public static GridManager Singleton;

//     [SerializeField] private Tilemap tilemap;
//     [SerializeField] private Tile placeholderTile;
// 	public TileBase GetTile(Vector3Int pos) => tilemap.GetTile(pos);	
//     void Awake()
//     {
//         Singleton = this;

//         // Populate walls with placeholders on breakables
//         var breakables = FindObjectsByType<Breakable>(FindObjectsSortMode.None);
// 		foreach (var b in breakables)
// 		{
// 			var pos = Vector3Int.FloorToInt(b.transform.position);
// 			tilemap.SetTile(pos, placeholderTile);
//         }

// 		var moveables = FindObjectsByType<Moveable>(FindObjectsSortMode.None);
// 		foreach (var m in moveables)
// 		{
// 			var pos = Vector3Int.FloorToInt(m.transform.position);
// 			tilemap.SetTile(pos, placeholderTile);
// 		}
// 		// Populate entities layer for non-player Entities
// 		var entities = FindObjectsByType<Entity>(FindObjectsSortMode.None);
//         foreach (var e in entities)
//         {
//             if (e is Player) continue;

//             var pos = Vector3Int.FloorToInt(e.transform.position);
//             tilemap.SetTile(pos, placeholderTile);
//         }
//     }

//     // public void MoveFloorTile(Vector3Int start, Vector3Int end)
//     // {
//     //     MoveTile(floor, start, end);
//     // }

//     public void MoveTile( Vector3Int start, Vector3Int end)
//     {
//         var tile = tilemap.GetTile(start);
//         if (tile == null) return;

//         tilemap.SetTile(start, null);
//         tilemap.SetTile(end, tile);
//     }

//     void OnDestroy()
//     {
//         // Clear all tile data
//         tilemap.ClearAllTiles();

//         // Unload any unused assets (optional but helps free memory)
//         Resources.UnloadUnusedAssets();
//     }
// }
