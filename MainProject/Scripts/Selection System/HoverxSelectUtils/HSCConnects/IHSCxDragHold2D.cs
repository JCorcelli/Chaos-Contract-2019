using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragHold2D : IHSCxConnect {
		
		
		public Transform heldThing;
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 5f;
		public bool pressed = false;
		
		protected override void OnEnable(){
			base.OnEnable();
			if (heldThing == null ) heldThing = this.transform;
			
			if (ih == null) return;
			ih.onRelease += Release;
			ih.onPress += Press;
		}
		protected override void OnDisable(){
			base.OnDisable();
			
			if (ih == null) return;
			ih.onRelease -= Release;
			ih.onPress -= Press;
		}
		
		protected void Press(HSCxController caller) {
			lastMousePosition = Input.mousePosition;
			
			
			hitOffset =   heldThing.position - lastMousePosition; // from its center, my offset
			
			pressed = true;
			
		}
		
		protected void Release(HSCxController caller) {
			pressed = false;
		}
		
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			if (!pressed) return;
			//float scaleFactor = Screen.width / 800f;
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			//delta /= scaleFactor;
			
			if (delta.magnitude > deltaSpeed) 
				lastMousePosition += delta.normalized * deltaSpeed;// * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			Vector3 anchoredPosition = lastMousePosition;
			
			
			
			
			heldThing.position = anchoredPosition + hitOffset ;
				
			
			
		}
		 
	}
}