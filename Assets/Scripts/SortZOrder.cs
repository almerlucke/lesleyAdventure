using UnityEngine;
using System.Collections;

public class SortZOrder : MonoBehaviour {
	//the camera looking at the sprite
	Camera cam;
	//the local transform for speed
	Transform trans;
	
	void Start()
	{
		cam = Camera.main;
		trans = transform;

		Update ();
	}
	
	void Update()
	{
		//don't run if sprite is not visible to any camera
		if(renderer && !renderer.isVisible)
			return;

		//store the viewport position of this GameObject.  
		Vector3 viewportPos = cam.WorldToViewportPoint(trans.position);
		
		//now we convert our position in the viewport to a usable number for the z order.    
		//This will always give us a number between 0 and 5. based on the Y value from the viewportPos.
		//0 will be the top of the screen and 5 would be the bottom. In the isometric view, objects higher on the screen would be "behind" objects lower on the screen.
		float newZPos = ConvertScale(viewportPos.y, 0, 1, 5, 0); //See my convert scale method below
		 
		//change the Z position of the GameObject to the newly calculated position between 0 - 5.  
		trans.position = new Vector3(trans.position.x, trans.position.y, -newZPos);
	}
	
	//this is a static float I use to convert scales.  This is normally placed in a helper script, but i'll show you it here.
	public static float ConvertScale (float old_value, float old_min, float old_max, float new_min, float new_max)
	{
		float new_value = ((old_value - old_min) / (old_max - old_min)) * (new_max - new_min) + new_min;
		return new_value;
	}
}
