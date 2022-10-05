using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	public class RotateAround2D : SelectAbstract {
		
		public Transform heldThing;
		public Transform rotateAround;
		
		protected Vector3 anchorPoint;
		protected Vector3 lastMousePosition ;
		
		
		
		
		
		protected override void OnPress() {
			base.OnPress();
			
			lastMousePosition = Input.mousePosition;
			anchorPoint = rotateAround.position;
			
			
			
			SelectGlobal.uiSelect = true;
			
			
		}
		protected override void OnLateUpdate() {
			
			base.OnLateUpdate();
			if (!pressed) return;
			
			
			Vector3 oldDirection = lastMousePosition - rotateAround.position ; // from mouse position to rotate or vice-versa
			
			lastMousePosition = Input.mousePosition;
			
			Vector3 newDirection = lastMousePosition - rotateAround.position ; // from mouse position to rotate or vice-versa
			
			
			float oldAngle = Mathf.Atan2(oldDirection.x, oldDirection.y) *  Mathf.Rad2Deg;
			
			float newAngle = Mathf.Atan2(newDirection.x, newDirection.y) *  Mathf.Rad2Deg;
			
			float change = newAngle - oldAngle;
			
			// axis should make z rotate
			// point is anchor?
			// angle is calculated
			heldThing.RotateAround(anchorPoint, Vector3.forward, -change); 
		}
	}
}