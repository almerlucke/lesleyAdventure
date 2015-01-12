using UnityEngine;
using System.Collections;

public class LesleyThePonyScript : InteractiveObject {
	float rotationY;
	bool wasWalking = false;
	public float speed = 4;
	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		rotationY = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 velocity = new Vector2();
		bool isWalking = false;

		if (Input.GetKey (KeyCode.UpArrow))
		{
			velocity.y = speed;
			isWalking = true;
		} 
		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			velocity.y = -speed;
			isWalking = true;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			velocity.x = -speed;
			rotationY = 0;
			isWalking = true;
		}
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			velocity.x = speed;
			rotationY = 180;
			isWalking = true;
		}

		transform.eulerAngles = new Vector3(0, rotationY, 0);
		rigidbody2D.velocity = velocity;

		if (isWalking) 
		{
			if (!wasWalking) 
			{
				Singleton.current.PlaySound(Singleton.SoundClipType.LesleyWalking);
			}

			animator.SetInteger ("state", 1);
			wasWalking = true;
		}
		else 
		{
			if (wasWalking) 
			{
				Singleton.current.PauseSound(Singleton.SoundClipType.LesleyWalking);
			}

			animator.SetInteger ("state", 0);
			wasWalking = false;
		}
	}

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}
	
	override public bool Activate()
	{
		Singleton.current.ShowMessageOverlay("Lesley is already activated, try moving him around with the arrow keys.");
		
		return true;
	}
	
	override public bool LookAt()
	{
		Singleton.current.ShowMessageOverlay("Yes that is Lesley the Pony, the protagonist of this game.");
		
		return true;
	}
}
