using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
public class CollectableData
{
	public CollectableData(string id, SpriteRenderer renderer, GameObject prefab = null)
	{
		this.id = id;
		this.renderer = renderer;
		this.prefab = prefab;
	}
	public string id;
	public SpriteRenderer renderer;
	public GameObject prefab;
}
public class Inventory : MonoBehaviour
{
	[SerializeField] GameObject inventoryParent;
	List<CollectableData> datas;
	void Awake()
	{
		datas = new();
	}
	public void AddCollectable(CollectableData data)
	{
		InventoryUI.Singleton.AddItem(data);
		datas.Add(data);

	}
	public void RemoveCollectable(CollectableData data)
	{
		InventoryUI.Singleton.RemoveItem(data);
		datas.Remove(data);
	}
	public CollectableData GetCollectableData(string id)
	{
		foreach (var c in datas)
			if (c.id == id)
				return c;
		return null;
	}
	public CollectableData GetAndRemoveCollectableData(string id)
	{
		var data = GetCollectableData(id);
		if (data != null)
			RemoveCollectable(data);
		return data;
	}
	
}
