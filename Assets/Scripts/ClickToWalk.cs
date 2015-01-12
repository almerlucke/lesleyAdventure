using UnityEngine;
using System.Collections;

public class ClickToWalk : MonoBehaviour {
	Vector3 targetPosition;
	float targetDistance;
	float moveSpeed;
	bool mouseClicked;

	// Use this for initialization
	void Start () {
		moveSpeed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		targetDistance = Vector3.Distance(targetPosition, transform.position);

		if(targetDistance < 2){ // prevents shaking when it reaches location
			moveSpeed = 0;
			mouseClicked = false;
		}
		else if(targetDistance > 2){
			moveSpeed = 10;
		}

		if (Input.GetKey (KeyCode.Mouse0))
		{
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseClicked = true;
		}

		if(mouseClicked){ // Prevents code running when it doesn't need to
			transform.position += (targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime;
		}
	}
}
