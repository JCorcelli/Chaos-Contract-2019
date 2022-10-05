using System;
using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	
    public class PlaceCanvasScaledMagnet : AbstractButtonHandler
    {
		// place a 2D object on screen and call a delegate
		public static bool locked = false;
		public IndicatorDelegate onDrag;
		public IndicatorDelegate onRelease;
		public RectTransform target;
		
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 5f;
		
		
		
		protected override void OnPress(){
			lastMousePosition = Input.mousePosition;
			UseButton();
		}
        protected override void OnHold()
		{
			UseButton();
		}
		
        protected override void OnRelease() {
			
			if (onRelease != null) onRelease();
		}
		
        protected void UseButton()
        {
			
			if (locked )
				return;
			
			float scaleFactor = transform.lossyScale.y;
			
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			
			if (delta.magnitude > deltaSpeed ) 
				lastMousePosition += delta.normalized * deltaSpeed  * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			target.anchoredPosition = lastMousePosition / scaleFactor;
			
			
			if (onDrag != null) onDrag();
        }
    }
}
