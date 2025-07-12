using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] GameObject itemPrefab;
	public void AddItem(CollectableData data)
	{
		var o = Instantiate(itemPrefab, transform);
		ItemUI i = o.GetComponent<ItemUI>();
		i.Init(data);
		
	}
	public void RemoveItem(CollectableData data)
	{
		foreach (Transform child in transform)
		{

			CollectableData d = child.GetComponent<ItemUI>().data;
			if (d == data)
			{
				Destroy(child.gameObject);
				return;
			}
		}
		Debug.Log("Could not find item " + data.ToString());
	}

	public static InventoryUI Singleton;
	void Awake() => Singleton = this;
}
