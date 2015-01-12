using UnityEngine;
using System.Collections;

public class CheckenScript : InteractiveObject 
{
	Vector2 velocity;
	Vector2 oldVelocity;
	float oldAnimatorSpeed;
	bool pluckedFeather;
	Animator animator;
	bool frozen;

	// Use this for initialization
	void Start () {
		RandomVelocity();
		UpdateRotation();

		animator = GetComponent<Animator>();
	}

	float RandomDirection()
	{
		float r = Random.Range(0.0f, 1.0f);

		if (r >= 0.5f) 
		{
			return 1.0f;
		}

		return -1.0f;
	}

	void RandomVelocity()
	{
		velocity = new Vector2(0.5f * RandomDirection(), Random.Range(0.001f, 0.4f) * RandomDirection());
	}

	void UpdateRotation()
	{
		float rotationY = 0.0f;

		if (velocity.x > 0.0f) 
		{
			rotationY = 180.0f;
		}
		
		transform.eulerAngles = new Vector3(0.0f, rotationY, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (frozen) return;

		int r = Random.Range(0, 2500);
		
		if (r < 10) 
		{
			if (Random.Range(0.0f, 1.0f) >= 0.2) 
			{
				Singleton.current.PlaySound(Singleton.SoundClipType.ChickenCalm);
			}
			else 
			{
				Singleton.current.PlaySound(Singleton.SoundClipType.ChickenAnxious);
			}

			RandomVelocity();
			UpdateRotation();
		}

		rigidbody2D.velocity = velocity;
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (frozen) return;

		ContactPoint2D hit = coll.contacts[0];

		velocity = Vector3.Reflect(velocity, hit.normal);

		UpdateRotation();
	}

	void ShowChickenMessage()
	{
		pluckedFeather = true;

		Singleton.current.ShowMessageOverlay("You picked a feather from the chicken. The chicken doesn't like you.");

		Inventory.current.AddItem(InventoryItem.InventoryItemType.Feather);

		// set to default layer to receive mouse events again
		gameObject.layer = 0;
	}

	override public void Freeze()
	{
		frozen = true;
		rigidbody2D.velocity = Vector2.zero;
		oldAnimatorSpeed = animator.speed;
		animator.speed = 0f;
	}

	override public void Thaw()
	{
		frozen = false;
		animator.speed = oldAnimatorSpeed;
	}

	override public bool LookAt()
	{
		if (!pluckedFeather)
		{
			Singleton.current.ShowMessageOverlay("The chicken walks around aimlessly, he has nice feathers though!");
		}
		else 
		{
			Singleton.current.ShowMessageOverlay("The chicken walks around aimlessly, he is naked.");
		}

		return true;
	}

	override public bool Activate()
	{
		if (!pluckedFeather)
		{
			animator.SetTrigger("nakedChecken");

			// set to raycast layer to prevent object receiving mouse events
			gameObject.layer = 2;
			
			Singleton.current.PlaySound(Singleton.SoundClipType.FairyDust);
            
            Invoke ("ShowChickenMessage", 0.5f);
		} 

		Singleton.current.PlaySound(Singleton.SoundClipType.ChickenAnxious);
		
		velocity = new Vector2(Random.Range(1.1f, 3.4f) * RandomDirection(), Random.Range(0.001f, 0.4f) * RandomDirection());
		
		UpdateRotation();

		return true;
    }

	override public bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}
}
