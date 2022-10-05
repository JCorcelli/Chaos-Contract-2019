using UnityEngine;
using UnityEngine.UI;

using System.Collections;


namespace SelectionSystem
{
	
	
	public class DragHold2DAmbience : DragHold2D {
		
		
		public void OnInstance(){
			lastMousePosition = Input.mousePosition;
			
			if (heldThing == null ) heldThing = this.transform;
			
			hitOffset =   Vector3.zero; // from its center, my offset
			
		}
		protected override void OnLateUpdate() {
			
			if (!pressed ) 
			{
				return;
			}
			//if (!pressed) return;
			SetVariables();
			SetDrag();
			UpdateBounds();
		}
		
	}
}