using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour 
{
	public InventoryItemType type;

	public bool isActive
	{
		get { return gameObject.activeSelf; }
		set 
		{
			gameObject.SetActive(value);
		}
	}

	private bool _selected;

	private Inventory _inventory;

	public bool selected
	{
		get { return _selected; }
		set 
		{
			_selected = value;

			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			if (_selected)
			{
				renderer.sprite = data.selectedSprite;
			}
			else 
			{
				renderer.sprite = data.sprite;
			}

			SetCursor();
		}
	}

	private InventoryItemData _data;

	public InventoryItemData data 
	{
		get { return _data; }
		set 
		{
			_data = value;
			type = _data.type;

			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			renderer.sprite = _data.sprite;
		}
	}

	public enum InventoryItemType
	{
		None,
		Feather,
		Key,
		Elf
	}

	public void SetCursor()
	{
		if (selected)
		{
			Cursor.SetCursor(data.cursorTexture, data.cursorHotspot, CursorMode.Auto);
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	public void AddToInventory(Inventory inventory)
	{
		_inventory = inventory;
	}

	void OnMouseUpAsButton()
	{
		_inventory.selectedItem = this;

		selected = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
