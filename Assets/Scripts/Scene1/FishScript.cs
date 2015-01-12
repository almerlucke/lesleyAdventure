using UnityEngine;
using System.Collections;

public class FishScript : InteractiveObject 
{
	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}

	override public bool LookAt()
	{
		Singleton.current.ShowMessageOverlay("A huge fish hangs above the door. It looks like a red herring...");

		return true;
	}

	override public bool Activate()
	{
		return false;
	}
}
