using System;
using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	
    public class ToolFreeBrushSnapping : AbstractButtonComboPrecision
    {
		
		
		public RectTransform toolObject;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 20f;
		
		public Transform snap;
		
		protected UISelectMask sMask;
		
		protected override void OnEnable() {
			base.OnEnable();
			if (snap == null) Debug.Log("needs snap", gameObject);
			sMask = GetComponentInParent<UISelectMask>();
			
			if (toolObject == null) toolObject = GetComponent<RectTransform>();
			
			if (toolObject == null) Debug.Log("needs toolObject", gameObject);
			sMask = GetComponentInParent<UISelectMask>();
			
			toolObject.gameObject.SetActive(false);
		}
		
		protected bool pressed = false;
		protected override void OnRelease() {
			pressed = false;
			toolObject.gameObject.SetActive(false);
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
			
			toolObject.gameObject.SetActive(true);
			
			lastMousePosition = snap.position;
			UseButton();
		}
        protected override void OnHold()
		{
			if (!pressed) return;
			UseButton();
		}
		 
		 
        protected void UseButton()
        {
			
			
			//float scaleFactor = Screen.width / 800f;
			
			Vector3 currentPosition = snap.position;
			Vector3 delta = currentPosition-lastMousePosition;
			
			
			if (delta.magnitude > deltaSpeed ) 
				lastMousePosition += delta.normalized * deltaSpeed   ;
			else
				lastMousePosition = currentPosition;
			
			toolObject.position = snap.position;
			
        }
    }
}
