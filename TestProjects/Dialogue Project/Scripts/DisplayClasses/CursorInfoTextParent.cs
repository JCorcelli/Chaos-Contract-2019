using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SelectionSystem
{
	public class CursorInfoTextParent : CursorInfo {
		// basically a class scattered around into nodes. text. aka CursorNode
		

		
		protected override void OnEnable () { 
			base.OnEnable();
			
			

		}
		
		
		protected override void OnExit() { 
			base.OnExit();
			isFocused = false;
			if (this == currentCursorInfo)
			{
				//cursorDetachable = true;
				//releaseCursor = true;
				cursorDetachable = true; // does it move at all?
				cursorEffect = "Circle";
			}
				
		}
		protected override void OnEnter() { 
			base.OnEnter();
			showCursor = true; // basically, show the cursor
			currentCursorInfo = this;
			
			cursorRefocus = true;
			
			cursorColor = color;
			cursorEffect = "Circle";
		}
		protected override void OnUpdate( ) {
			base.OnUpdate();
			
			// if active focused = true;
			if (!isHovered) 
			{
				return;
			}
			// idk
			// Main();
				
		}
		
		protected override void OnPress(){
			
			// focus?	
			
		}
	}
}