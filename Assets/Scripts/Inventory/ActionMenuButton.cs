using UnityEngine;
using System.Collections;

public class ActionMenuButton : MonoBehaviour 
{
	public ActionMenu.ActionMenuState stateRepresentation;

	private ActionMenu menu;

	void Start () 
	{
		menu = transform.parent.gameObject.GetComponent<ActionMenu>();
	}

	void OnMouseEnter()
	{
		menu.state = stateRepresentation;
	}

	void OnMouseExit()
	{
		menu.state = ActionMenu.ActionMenuState.None;
	}
//
//	void OnMouseUpAsButton()
//	{
//		menu.Action(stateRepresentation);
//	}
}
