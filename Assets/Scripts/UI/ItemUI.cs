using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
	public CollectableData data;
    [SerializeField] Image image;
	public void Init(CollectableData data)
	{
		this.data = data;
		image.sprite = data.sprite;
	}

}
