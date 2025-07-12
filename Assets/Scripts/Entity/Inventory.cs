using System.Collections.Generic;
using UnityEngine;
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
		if (data)
			RemoveCollectable(data);
		return data;
	}
	
}
