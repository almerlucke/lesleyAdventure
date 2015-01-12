using UnityEngine;
using System.Collections;

public class ElfScript : InteractiveObject 
{
	private Singleton singleton;
	private Animator animator;
	public GameObject deadElf;
	public GameObject evilBook;
	private ParticleSystem bloodParticleSystem;

	void Start()
	{
		singleton = Singleton.current;
		animator = GetComponent<Animator>();

		bloodParticleSystem = transform.Find("torso").GetComponent<ParticleSystem>();
		bloodParticleSystem.transform.eulerAngles = new Vector3(-40, 180, 0);
		bloodParticleSystem.Stop();
	}

	void ShowMinorTickleMessage()
	{
		singleton.ShowMessageOverlay("The elf giggles when you touch him. What if you could tickle it a little bit more?");
	}

	void ShowDeadElf()
	{
		singleton.ShowMessageOverlay("You tickled the elf to dead. You hear a sound coming from the bookcase.");

		evilBook.SetActive(true);

		deadElf.SetActive(true);
		
        Destroy(gameObject);

		Inventory.current.RemoveItem(InventoryItem.InventoryItemType.Feather);
	}

	void ShowBlood()
	{
		bloodParticleSystem.Play();

		Invoke("ShowDeadElf", 2.5f);
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		if (itemType == InventoryItem.InventoryItemType.Feather)
		{
			// set to raycast layer to prevent object receiving mouse events
			gameObject.layer = 2;
			
			singleton.PlaySound(Singleton.SoundClipType.ElfDying);
			
			animator.SetTrigger("die");
			
			Invoke("ShowBlood", 1.5f);
			
			return true;
		}
		
		return false;
	}

	override public bool LookAt()
	{
		singleton.ShowMessageOverlay("There is an annoying looking elf standing in front of a chest");

		return true;
	}

	override public bool Activate()
	{
		singleton.PlaySound(Singleton.SoundClipType.ElfGiggle);
		
		animator.SetTrigger("giggle");
		
		Invoke("ShowMinorTickleMessage", 1.2f);

		return true;
	}
}
