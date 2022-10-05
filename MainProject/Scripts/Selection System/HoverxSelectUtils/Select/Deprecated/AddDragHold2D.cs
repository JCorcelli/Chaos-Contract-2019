using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	public class AddDragHold2D : SelectRelayAddAbstract {
		
		
		public Transform heldThing;
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 50f;
		
		
		protected override void OnEnable(){
			base.OnEnable();
			if (heldThing == null ) heldThing = this.transform;
			
		}
		
		protected override void OnPress() {
			base.OnPress();
			ih.used = true;
			
			lastMousePosition = Input.mousePosition;
			
			
			hitOffset =   heldThing.position - lastMousePosition; // from its center, my offset
			
			
		}
		
		
		
		protected override void OnLateUpdate() {
			
			base.OnLateUpdate();
			if (!ih.pressed) return;
			
			if (ih.used) return;
			ih.used = true;
			
			
			
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			//float scaleFactor = Screen.width / 800f;
			//delta /= scaleFactor;
			if (delta.magnitude > deltaSpeed) 
				lastMousePosition += delta.normalized * deltaSpeed; // * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			Vector3 anchoredPosition = lastMousePosition;
			
			
			
			
			heldThing.position = anchoredPosition + hitOffset ;
				
			
			
		}
		 
	}
}