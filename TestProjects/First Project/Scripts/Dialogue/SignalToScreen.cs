using UnityEngine;  

using System.Collections;  

public class SignalToScreen : MonoBehaviour  
{  
	public Transform signalOrigin;
	public RectTransform imageTransform;
	public UnityEngine.UI.Image image;
	//this game object's transform  
	private Transform goTransform;  
	//the game object's position on the screen, in pixels  
	private Vector3 goScreenPos;  
	
	
	public int normWidth = 100;
	public int normHeight = 100;
	public int smallWidth = 50;
	public int smallHeight = 50;
	
	public float focalAngle = 80f;
	
	
	//Called once per frame, after the update  
	void LateUpdate()  
	{  
		
		// check if facing
		float visibility = -Vector3.Dot((Camera.main.transform.position - signalOrigin.position).normalized , Camera.main.transform.forward);
		bool facing = visibility > focalAngle/180f;
		bool zerofacing = visibility > 0;
	
		goScreenPos = Camera.main.WorldToScreenPoint(signalOrigin.position);  	
		if (facing) // I am angled in a way I could see it
		{
			imageTransform.sizeDelta = new Vector2( normWidth, normHeight);
			
			imageTransform.anchoredPosition = goScreenPos;
		}
		else
			
		{
			// I'm looking at the mirror so I have to flip the screen
			
			imageTransform.sizeDelta = new Vector2( smallWidth, smallHeight);
			
			bool noflip = false;
			if (!zerofacing)
				goScreenPos.x = goScreenPos.x * -1 + Screen.width;
			else
				noflip = true;
			
			if (goScreenPos.x > Screen.width - smallWidth)
				goScreenPos.x = Screen.width - smallWidth;
			else if (goScreenPos.x < 0)
				goScreenPos.x = 0;
			else
			{
				goScreenPos.y = 0;
				noflip = true;
			}
			if (!noflip) goScreenPos.y = 0;
			else if (goScreenPos.y < 0) goScreenPos.y = 0;
			imageTransform.anchoredPosition = goScreenPos;
			
		}
		
		//find out the position on the screen of this game object   
		
		
	}  
	
}