using UnityEngine;
using System.Collections;

public class ActionMenu : MonoBehaviour {
	private ActionMenuState _state;

	public Sprite[] stateSprites;

	public GameObject targetObject;

	public bool isShown 
	{
		get
		{
			return gameObject.activeSelf;
		}
	}

	public ActionMenuState state
	{
		get { return _state;}
		set 
		{
			_state = value;

			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			renderer.sprite = stateSprites[(int)_state];
		}
	}

	public enum ActionMenuState
	{
		None,
		Look,
		Touch
	}

	// Use this for initialization
	void Start () 
	{
		state = ActionMenuState.None;
	}

	public void HandleMouseUp(Vector2 mousePosition)
	{
		foreach (Transform child in transform)
		{
			GameObject childGameObject = child.gameObject;
			Vector3 mousePositionWithZ = new Vector3(mousePosition.x, mousePosition.y, childGameObject.transform.position.z);
			Bounds bounds = childGameObject.collider2D.bounds;

			if (bounds.Contains(mousePositionWithZ))
			{
				ActionMenuButton actionMenuButton = childGameObject.GetComponent<ActionMenuButton>();

				Action(actionMenuButton.stateRepresentation);

                break;
            }
		}
    }

	public void ShowMenu(Vector2 mousePosition, GameObject selectedGameObject)
	{
		targetObject = selectedGameObject;
		
		float posX = mousePosition.x;
		float posY = mousePosition.y;
		
		if (selectedGameObject.tag == "InteractionObject")
		{
			InteractiveObject interactiveObject = selectedGameObject.GetComponent<InteractiveObject>();
			Bounds bounds = interactiveObject.Bounds();
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;
			
			if (center.x < 0)
			{
				posX = center.x + extents.x * 0.25f + renderer.bounds.extents.x;
			}
			else
			{
				posX = center.x - extents.x * 0.25f - renderer.bounds.extents.x;
			}

			interactiveObject.Freeze();
		}
		
		transform.position = new Vector3(posX,
		                                 posY,
		                                 transform.position.z);

		gameObject.SetActive(true);
	}

	public void HideMenu()
	{
		InteractiveObject interactiveObject = targetObject.GetComponent<InteractiveObject>();

		if (interactiveObject)
		{
			interactiveObject.Thaw();
		}

		gameObject.SetActive(false);
	}
    
    public void Action(ActionMenuState actionState)
    {
        InteractiveObject interactiveObject = targetObject.GetComponent<InteractiveObject>();

		bool canInteract = false;
		
		if (interactiveObject) 
		{
			if (actionState == ActionMenuState.Look)
            {
				canInteract = interactiveObject.LookAt();
			} 
			else if (actionState == ActionMenuState.Touch)
			{
				canInteract = interactiveObject.Activate();
			}
		}

		if (!canInteract)
		{
			if (actionState == ActionMenuState.Look)
			{
				Singleton.current.ShowMessageOverlay("Nothing to see, move along.");
			} 
			else if (actionState == ActionMenuState.Touch)
			{
				Singleton.current.ShowMessageOverlay("Nothing happened.");
            }
		}

		HideMenu();
    }
}
