using UnityEngine;
using System.Collections;

public class WindowScript : InteractiveObject 
{
	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}
	
	override public bool LookAt()
	{
		Singleton.current.ShowMessageOverlay("Lesley admires the scenery.");
		
		return true;
	}
	
	override public bool Activate()
	{
		return false;
	}
}
