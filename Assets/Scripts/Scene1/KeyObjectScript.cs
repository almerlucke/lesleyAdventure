using UnityEngine;
using System.Collections;

public class KeyObjectScript : InteractiveObject 
{
	void Start()
	{
		gameObject.SetActive(false);
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}
	
	override public bool LookAt()
	{
		Singleton.current.ShowMessageOverlay("The elf is sacrificed and transformed into a key.");
		
		return true;
	}
	
	override public bool Activate()
	{
		Singleton.current.PlaySound(Singleton.SoundClipType.FairyDust);

		Inventory.current.AddItem(InventoryItem.InventoryItemType.Key);

		Destroy(gameObject);

		return true;
	}
}
