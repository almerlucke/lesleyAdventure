using UnityEngine;
using System.Collections;

public class DoorScript : InteractiveObject {
	protected Animator animator;
	public bool isDoorOpen;
	public bool killedTheDuke;
	public GameObject bloodObject;
	public GameObject lesley;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		if (killedTheDuke) return true;

		if (itemType == InventoryItem.InventoryItemType.Key)
		{
			// set to raycast layer to prevent object receiving mouse events
			gameObject.layer = 2;
			
			Singleton.current.PlaySound(Singleton.SoundClipType.UnlockDoor);
			
			Invoke("UnlockDoorAction", 2.0f);

			return true;
		}

		return false;
	}
	
	override public bool LookAt()
	{
		if (killedTheDuke) return true;

		if (isDoorOpen) 
		{
			Singleton.current.ShowMessageOverlay("The door is open, maybe you should walk inside for revenge!");
		} 
		else 
		{
			Singleton.current.ShowMessageOverlay("A huge wooden door. Behind this door the Duke is cowardly hiding.");
		}
		
		return true;
	}
	
	override public bool Activate()
	{
		if (killedTheDuke) return true;

		if (isDoorOpen) 
		{
			Singleton.current.ShowMessageOverlay("The door is already open.");
		} 
		else 
		{
			Singleton.current.ShowMessageOverlay("The door won't budge. Maybe a key is needed to open this door.");
		}

		return true;
	}

	void OpenDoorAction()
	{
		Singleton.current.ShowMessageOverlay("You unlocked the door. Step right through for sweet revenge!");

		// set to default layer to receive mouse events again
		gameObject.layer = 0;

		Inventory.current.RemoveItem(InventoryItem.InventoryItemType.Key);
	}

	void UnlockDoorAction()
	{
		animator.SetTrigger("openDoor");

		isDoorOpen = true;

		Singleton.current.PlaySound(Singleton.SoundClipType.OpenDoor);

		Invoke("OpenDoorAction", 1.5f);
	}

	void FinishLevel()
	{
		Application.LoadLevel("EndScene");
	}

	void KillTheDuke()
	{
		Animator bloodAnimator = (Animator)bloodObject.GetComponent<Animator>();

		bloodObject.SetActive(true);
		bloodAnimator.SetTrigger("flow");

		Invoke("FinishLevel", 6.0f);
	}

	public void CloseTheDoorAndKillTheDuke()
	{
		killedTheDuke = true;

		Singleton.current.PauseSound(Singleton.SoundClipType.LesleyWalking);

		Singleton.current.PlaySound(Singleton.SoundClipType.KillTheDuke);

		Destroy(lesley);

		animator.SetTrigger("closeDoor");

		Invoke("KillTheDuke", 3.0f);
	}
}
