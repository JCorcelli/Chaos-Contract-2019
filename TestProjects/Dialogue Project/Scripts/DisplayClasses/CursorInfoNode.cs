using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SelectionSystem
{
	public class CursorInfoNode : CursorInfo {
		// basically a class scattered around into nodes. text. aka CursorNode
		public RectTransform rectTransform;

		
		protected override void OnEnable () { 
			base.OnEnable();
			if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
			

		}
		
		
		protected override void OnExit() { 
			base.OnExit();
			isFocused = false;
			if (this == currentCursorInfo)
			{
				cursorDetachable = true;
				releaseCursor = true;
				cursorEffect = "Circle";
			}
				
		}
		protected override void OnEnter() { 
			base.OnEnter();
			showCursor = false;
			cursorDetachable = false; // idk
			currentCursorInfo = this;
			cursorRefocus = true;
			isFocused = true;
			
			cursorParent = rectTransform;
			cursorEffect = "Circle";
			cursorColor = color;
			cursorSize = defaultCursorSize;
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
			cursorPosition = mousePos;
				
		}
		
		protected override void OnPress(){
			
						
			
		}
	}
}