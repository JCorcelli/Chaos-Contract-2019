using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DialogueSystem;
using Utility.UI;
namespace SelectionSystem
{
	public class CursorInfoText : CursorInfo {
		// basically a class scattered around into nodes. text. aka CursorNode

		public RectTransform textTransform;
		public RectTransform backlightTransform;
		public MultiVertexImg backlight;
		
		public DisplayNode text;
		
		protected override void OnExit() { 
			base.OnExit();
			isFocused = false;
				
		}
		protected override void OnEnter() { 
			base.OnEnter();
			showCursor = true; // basically, show the cursor
			cursorParent = text.bodyframe;
			cursorRefocus = true;
			
		}
		
		
		protected override void OnEnable () { 
			base.OnEnable();
			
			// child is backlight
			if (text == null) text = GetComponentInParent<MemoryBoxNode>();
			if (backlightTransform == null) backlightTransform = backlight.GetComponent<RectTransform>();
			if (text == null) Debug.Log("where is the text node?", gameObject);
			
			if (textTransform == null) textTransform = text.body;

		}
		
		protected bool closeEnough = false;
		protected float xdistance = 0f;
		protected float ydistance = 0f;
		protected override void OnLateUpdate( ) {
			backlightTransform.position = textTransform.position;
			backlightTransform.sizeDelta = textTransform.sizeDelta;
		}
		protected override void OnUpdate( ) {
			base.OnUpdate();
			if (!isHovered) 
			{
				return;
			}
			// make sure the cursor will update
			
			// I have to rotate the cursor position around 
			Main();
			
			// close enough to touch as a rect
			
			if (wordText != "" )
			{
			
				cursorSize = text.hitChar.size;
				
				cursorPosition = text.hitChar.center; 
				
				cursorPosition = text.body.TransformPoint(cursorPosition);
				
				xdistance = Mathf.Abs(cursorPosition.x - mousePos.x) / scaleFactor;
				ydistance = Mathf.Abs(cursorPosition.y - mousePos.y) / scaleFactor;
				
				closeEnough = (
				xdistance < defaultCursorSize.x /2f
				&& ydistance < defaultCursorSize.y /2f
				);
				
			
				
			}
				
			if (closeEnough)
			{
				
				
				if (text.dirtySelection) backlight.Set(text.uiSelection);
				text.dirtySelection = false;
				isFocused = true;
				
				cursorEffect = "Square";
				cursorSize = new Vector2(2 / scaleFactor, lineHeight);
				
				cursorPosition = text.editPos; 
				
				cursorPosition = text.body.TransformPoint(cursorPosition);
			}
			else 
			{
				if (ydistance < defaultCursorSize.y  && xdistance < defaultCursorSize.x  )
					cursorEffect = "Square";
				else
					cursorEffect = "Circle";
					
				backlight.Clear();
				text.dirtySelection = true;
				// might be a blank space but idk right now
				isFocused = false;
				
				
			}
				
		}
		
		
		protected virtual void Main(){
			
			isFocused = text.CastPoint( mousePos);
			
			if (isFocused)
				cursorEffect = "Square";
			else 
			{
				cursorEffect = "Circle";
				
				return;
			}
			wordText = text.wordText.ToString();
			cursorColor = color; // = text.bodyText.color;	// can i invert it here?
			
			lineSize = text.hitLine.size;
			linePosition = 		text.hitLine.center;
			paragraphSize = 	text.hitParagraph.size;
			paragraphPosition = text.hitParagraph.center;
			wordSize = text.hitWord.size;
			wordPosition = text.hitWord.center;
			
			
			
			
			
			scaleFactor = textTransform.lossyScale.y;
			fontHeight = text.fontHeight;
			lineHeight = text.lineHeight;
			lineWidth = textTransform.sizeDelta.x;
			textTransform.GetWorldCorners(fourCorners);
			
			// there's also an editPos which is between chars
			
			
			
		}
		protected override void OnPress(){
						
						
			
		}
	}
}