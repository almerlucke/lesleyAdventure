using UnityEngine;
using System.Collections;

public class DoorEnterScript : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Lesley")
		{
			DoorScript doorScript = (DoorScript)GameObject.Find("Door").GetComponent("DoorScript");

			if (doorScript.isDoorOpen)
			{
				doorScript.CloseTheDoorAndKillTheDuke();
			}
		}
	}
}
