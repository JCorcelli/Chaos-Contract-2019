using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using SelectionSystem;

namespace Utility.Managers
{
	
	public class SelectHUB : AbstractTriggerHUB, IPointerDownHandler, IPointerUpHandler {

		public string buttonName = "mouse 1";
		protected bool pressed = false;
		public void  OnPointerDown( PointerEventData eventData ) {
			if (Input.GetButton( buttonName)) 
			{
				
				pressed = true;
			
				if (onTriggerEnter != null) onTriggerEnter();
			}
				
				
			}
		
		protected void OnDisable(){
			if (pressed)
			{
				pressed = false;
				if (onTriggerExit != null) onTriggerExit();
			}
			
		}
		public void  OnPointerUp( PointerEventData eventData )
		{
			if (!(pressed && !Input.GetButton(buttonName))) return;
			pressed = false;
			if (onTriggerExit != null) onTriggerExit();
			
		}
		void OnTriggerExit( Collider col ) {
		
			if (onTriggerExit != null) onTriggerExit();
			
			
		}
	}
}