using System;
using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	
    public class PlaceRectTool : AbstractButtonComboPrecision
    {
		
		
		protected RectTransform target;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float disableDistance = 2f;
		
		public GameObject toolObject;
		
		
		protected UISelectMask sMask;
		
		protected override void OnEnable() {
			base.OnEnable();
			sMask = GetComponentInParent<UISelectMask>();
			
			target = toolObject.GetComponent<RectTransform>();
			
		}
		
		
		protected bool pressed = false;
		protected override void OnRelease() {
			pressed = false;
		}
		protected override void OnPress(){
			if (sMask.isHovered) 
			{
				pressed = true;
			}
			else
			{
				pressed = false;
				return;
			}
			
			
			lastMousePosition = Input.mousePosition;
			toolObject.SetActive(true);
			target.position = lastMousePosition ;
			
		}
        protected override void OnHold()
		{
			if (!pressed) return;
			UseButton();
		}
		 
		 
        protected void UseButton()
        {
			
			
			//float scaleFactor = Screen.width / 800f;
			
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			
			
			if (delta.magnitude > disableDistance )
			{				
				toolObject.SetActive(false);
				bad_combo = true;
			}
			
			
			
        }
    }
}
