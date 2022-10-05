using System;
using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	
    public class KeyMoveRectTool : AbstractAnyHandler
    {
		
		
		protected RectTransform target;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float speed = 2f;
		public float shiftSpeed = 4f;
		
		public GameObject toolObject;
		
		
		public OnSelectFocusRelay sFocus;
		
		protected override void OnEnable() {
			base.OnEnable();
			sFocus = GetComponentInParent<OnSelectFocusRelay>();
			
			target = toolObject.GetComponent<RectTransform>();
			
			
		}
		
		
		protected override void OnRelease() {
			pressed = false;
		}
		
        protected override void OnHold()
		{
			if (!pressed) return;
			if (!sFocus.isFocused) 
			{
				return;
			}
			
			UseButton();
		}
		 
		 
        protected void UseButton()
        {
			
			
			//float scaleFactor = Screen.width / 800f;
			
			
			Vector3 delta = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0).normalized;
			
			
			if (Input.GetButton("shift")) delta *= shiftSpeed;
			else
				delta *= speed;
			
			lastMousePosition = target.position + delta;
			target.position = lastMousePosition ;
			
			
			
        }
    }
}
