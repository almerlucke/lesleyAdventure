using UnityEngine;
using System.Collections;

public class DeadElfScript : InteractiveObject 
{
	private bool pickedUp;

	void Start()
	{
		gameObject.SetActive(false);
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		if (pickedUp) return true;

		return false;
	}
	
	override public bool Activate()
	{
		if (pickedUp) return true;

		Singleton.current.PlaySound(Singleton.SoundClipType.FairyDust);

		Singleton.current.ShowMessageOverlay("You picked up the elf, what to do with it?");

		Inventory.current.AddItem(InventoryItem.InventoryItemType.Elf);

		pickedUp = true;

		gameObject.SetActive(false);
		
		return true;
	}
	
	override public bool LookAt()
	{
		if (pickedUp) return true;

		Singleton.current.ShowMessageOverlay("the elf is dead. Maybe you should hide the evidence.");
	
		return true;
	}
}
