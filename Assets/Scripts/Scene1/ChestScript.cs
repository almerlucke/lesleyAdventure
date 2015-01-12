using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChestScript : InteractiveObject {
	Animator anim;
//	bool pickedUpKey;
//	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}

	override public bool Activate()
	{
		bool chestOpen = anim.GetBool("ChestOpen");
		
		if (chestOpen)
		{
			Singleton.current.PlaySound(Singleton.SoundClipType.ChestClose);
			anim.SetBool("ChestOpen", false);
		}
		else 
		{
			Singleton.current.PlaySound(Singleton.SoundClipType.ChestOpen);
			anim.SetBool("ChestOpen", true);
		}

		return true;
	}

	override public bool LookAt()
	{
		bool chestOpen = anim.GetBool("ChestOpen");

		if (chestOpen)
		{
			Singleton.current.ShowMessageOverlay("Dang it, the chest is empty :(");
		}
		else 
		{
			Singleton.current.ShowMessageOverlay("An old chest. Looks promising!");
		}

		return true;
	}
}
