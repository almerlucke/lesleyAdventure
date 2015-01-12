using UnityEngine;
using System.Collections;

public class EvilBookScript : InteractiveObject 
{
	public Sprite darkBackgroundSprite;
	private Animator animator;
	private bool bookPulled;
	public GameObject sacrificeObject;

	void Start()
	{
		animator = GetComponent<Animator>();

		gameObject.SetActive(false);
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}
	
	override public bool Activate()
	{
		if (!bookPulled)
		{
			bookPulled = true;

			animator.SetTrigger("pullBook");

			GameObject background = GameObject.Find("Background");

			Animator backgroundAnimator = (Animator)background.GetComponent<Animator>();

			backgroundAnimator.SetTrigger("makeDark");

			Singleton.current.PlaySound(Singleton.SoundClipType.ThunderClap);

			sacrificeObject.SetActive(true);
		
			return true;
		}

		return false;
	}
	
	override public bool LookAt()
	{
		if (!bookPulled)
		{
			Singleton.current.ShowMessageOverlay("An evil looking book with a pentagram in it's cover.");
		}
		else 
		{
			Singleton.current.ShowMessageOverlay("By pulling the book you lifted the veil, and revealed the dark room.");
		}

		return true;
	}
}
