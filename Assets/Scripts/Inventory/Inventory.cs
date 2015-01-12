using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItemData
{
	public Texture2D cursorTexture;
	public Vector2 cursorHotspot;
	public Sprite sprite;
	public Sprite selectedSprite;
	public InventoryItem.InventoryItemType type;
}

public class Inventory : MonoBehaviour 
{
	public InventoryItemData[] availableItemData;

	public InventoryItem.InventoryItemType selectedItemType
	{
		get 
		{
			InventoryItem item = selectedItem;

			if (item)
			{
				return item.type;
			}

			return InventoryItem.InventoryItemType.None;
		}
	}

	public InventoryItem selectedItem
	{
		get 
		{
			return _selectedItem;
		}
		set 
		{
			if (_selectedItem)
			{
				_selectedItem.selected = false;
			}

			_selectedItem = value;
		}
	}

	public GameObject inventoryItemPrefab;

	public static Inventory current
	{
		get 
		{
			return (Inventory)GameObject.Find("Inventory").GetComponent("Inventory");
		}
	}

	private InventoryItem _selectedItem;

    private ArrayList allItems = new ArrayList();
	private ArrayList inactiveItems = new ArrayList();
	private ArrayList activeItems = new ArrayList();

	void Start () 
	{
		foreach (InventoryItemData itemData in availableItemData)
		{
			GameObject obj = (GameObject)Instantiate(inventoryItemPrefab);
			InventoryItem item = obj.GetComponent<InventoryItem>();

			item.data = itemData;
			item.isActive = false;
			item.AddToInventory(this);

			allItems.Add(item);
			inactiveItems.Add(item);
		}
	}

	public bool HasCollectedItem(InventoryItem.InventoryItemType itemType)
	{
		InventoryItem item = FindItem(itemType);

		if (item)
		{
			return item.isActive;
		}

		return false;
	}

	public InventoryItem FindItem(InventoryItem.InventoryItemType itemType)
	{
		foreach (InventoryItem item in allItems)
		{
			if (item.type == itemType)
			{
				return item;
			}
		}

		return null;
	}

	public void RemoveItem(InventoryItem.InventoryItemType itemType)
	{
		InventoryItem item = FindItem(itemType);

		if (item)
		{
			RemoveItem(item);
		}
	}

	public void RemoveItem(InventoryItem item)
	{
		item.isActive = false;

		if (item.selected)
		{
			selectedItem = null;
		}

		activeItems.Remove(item);
		inactiveItems.Add(item);
		UpdateItemPositions();
	}

	public void AddItem(InventoryItem.InventoryItemType itemType)
	{
		InventoryItem item = FindItem(itemType);
		
		if (item)
		{
            AddItem(item);
        }
	}

	public void AddItem(InventoryItem item)
	{
		item.isActive = true;

		inactiveItems.Remove(item);
		activeItems.Add(item);
		UpdateItemPositions();
	}

	void UpdateItemPositions()
	{
		float xOffset = -4.8f;

		foreach (InventoryItem item in activeItems)
		{
			item.transform.position = new Vector3(xOffset, -3.58f, -1.0f);
			xOffset += 1.05f;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			selectedItem = null;
		}
	}
}
