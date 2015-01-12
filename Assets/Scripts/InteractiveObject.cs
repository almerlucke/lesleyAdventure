using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour 
{
	public virtual bool InteractWithItem(InventoryItem.InventoryItemType itemType)
	{
		return false;
	}

	public virtual bool LookAt()
	{
		return false;
	}

	public virtual bool Activate()
	{
		return false;
	}

	public virtual Bounds Bounds()
	{
		Bounds result = new Bounds(Vector3.zero, Vector3.zero);

		if (gameObject.renderer)
		{
			result = gameObject.renderer.bounds;
		}
		else 
		{
			// use the bounds of the first trigger collider found
			Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

			foreach(Collider2D collider in colliders)
			{
				if (collider.isTrigger)
				{
					result = collider.bounds;
					break;
				}
			}
		}

		return result;
	}

	public virtual void Freeze()
	{

	}

	public virtual void Thaw()
	{

	}
}
