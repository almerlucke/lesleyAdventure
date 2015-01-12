using UnityEngine;
using System.Collections;

public class SacrificeScript : InteractiveObject 
{
	private bool droppedElf;
	private bool performedSacrifice;
	private ParticleSystem sacrificeParticleSystem;

	public GameObject deadElf;
	public GameObject keyObject;
	public GameObject sacrificeEffect;

	void Start()
	{
		gameObject.SetActive(false);

		sacrificeParticleSystem = sacrificeEffect.GetComponent<ParticleSystem>();
	}

	void EndSacrifice()
	{
		performedSacrifice = true;

		Destroy(deadElf);
		
		keyObject.SetActive(true);
	}

	void PerformSacrifice()
	{
		Singleton.current.PlaySound(Singleton.SoundClipType.SatanicChant);

		sacrificeParticleSystem.Play();

		Invoke("EndSacrifice", 4.5f);
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		if (itemType == InventoryItem.InventoryItemType.Elf)
		{
			if (!droppedElf)
			{
				Inventory.current.RemoveItem(InventoryItem.InventoryItemType.Elf);

				Vector2 center = collider2D.bounds.center;

				center.y -= deadElf.renderer.bounds.extents.y;
				
				deadElf.transform.position = center;
				deadElf.SetActive(true);

				droppedElf = true;

				PerformSacrifice();
			}

			return true;
		}

		return false;
	}
	
	override public bool LookAt()
	{
		if (droppedElf && !performedSacrifice)
		{
			Singleton.current.ShowMessageOverlay("The dead elf is lying in the circle waiting to be sacrificed.");
		}
		else
		{
			Singleton.current.ShowMessageOverlay("A sacrificial circle with a pentragram drawn in blood. Candles are lit around the circle.");
		}

		return true;
	}
	
	override public bool Activate()
	{
		if (!droppedElf)
		{
			Singleton.current.ShowMessageOverlay("You need something to sacrifice.");

			return true;
		}
		else if (!performedSacrifice)
		{
			return true;
		}

		return false;
	}
}
