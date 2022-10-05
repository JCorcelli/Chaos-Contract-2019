using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SelectionSystem
{
	public class CursorInfoTextUnity : CursorInfo {
		// basically a class scattered around into nodes. text. aka CursorNode

		public RectTransform textTransform;
		

		
// OTHER		
		public Text text;
		
		public char c;
		
		public int hitChar  = 0;
		
		
		protected override void OnEnable () { 
			base.OnEnable();
			
			// child is highlight
			if (text == null) text = GetComponent<Text>();
			if (text == null) Debug.Log("where is the text?", gameObject);
			
			if (textTransform == null) textTransform = text.gameObject.GetComponent<RectTransform>();

		}
		protected char[] chars;
		protected int endChar = 0;
		protected int startChar = 0;
		protected int hitLine = 0;

		protected TextGenerator tex;
		protected Vector2 center;
		protected Vector2 closestPos;
		
		protected override void OnUpdate( ) {
			base.OnUpdate();
			if (!isHovered) 
			{
				return;
			}
			
			// make sure the cursor will update
			
			// I have to rotate the cursor position around 
			cursorColor = color;
			Main();
			
			// close enough to touch as a rect
			
			if (wordText != "" )
			{
				if (wordSize.x < wordSize.y) cursorSize.x = defaultCursorSize.x; 
				else cursorSize.x = wordSize.x;
				
			}
			bool closeEnough = (
			distance < cursorSize.x /2f
			&& distance < wordSize.y /2f
			);
			if (closeEnough && wordText != "")
			{
				
				cursorSize = wordSize;
				cursorPosition = 	wordPosition;
				isFocused = true;
				cursorEffect = "Square";
			}
			else 
			{
				// might be a blank space but idk right now
				isFocused = false;
				
				if (distance > defaultCursorSize.x / 2f )
					cursorEffect = "Circle";
				else
					cursorEffect = "Square";
				
			}
				
		}
		
		protected int indexLimit = 0;
		protected float distance = 0;
		protected virtual void Main(){
			
			tex = text.cachedTextGenerator;
			if (tex == null ||  tex.characters.Count < 1 ) return;
			
			chars = 
				text.text.ToCharArray();
			if (chars.Length < 1 ) return;
			
			
			
			indexLimit = (tex.characters.Count < chars.Length) ? tex.characters.Count : chars.Length;
			
			scaleFactor = textTransform.lossyScale.y;
				
			
			// this works to fix line spacing
			lineHeight = tex.lines[0].height;
			//fontHeight  ;

			fontHeight = lineHeight / text.lineSpacing ; // for letter height only
			
			
			textTransform.GetWorldCorners(fourCorners);
			
			center = 
			textTransform.position ;
			
			// for margins
			// Vector3 up = textTransform.up;
			// Vector3 right = textTransform.right; 
			
			
			
			lineWidth = textTransform.sizeDelta.x;
			// line from \n to \n? this would be wrong
			
			FindLine();
			
			int closestChar = 0;
			// get line by char position
			Vector2 nextPos ;
			
			distance = -1f;
			float nextDistance = -1f;
			closestPos = tex.characters[0].cursorPos + center; 
			closestPos.x += tex.characters[0].charWidth / 2f;
			closestPos.y -= fontHeight / 2f - top;
			//closestPos.y -= lineHeight *.5f ;
			
			distance = Vector2.Distance(closestPos, mousePos);
			
			
			
			// walk all the characters
			for (int i = 0; i < indexLimit; i++)
			{
				nextPos = tex.characters[i].cursorPos + center ;
				nextPos.x += tex.characters[i].charWidth / 2f;
				nextPos.y -= fontHeight / 2f - top;
				
				nextDistance = Vector3.Distance(nextPos, mousePos);
				
				if (nextDistance < distance )	
				{
					// alternative isa hotspot and return all it touches, then pick closest.
					distance = nextDistance;
					closestChar = i;
					closestPos = nextPos;
				}
				
			}
			// so.... this is the line
			hitLine = 0;	
			
			for (int i = 1; i < tex.lines.Count; i++)
			{
				
				if (tex.lines[i].startCharIdx > closestChar)
				{
					break;
				}
				hitLine = i;
			}
			//LINE
			
			linePosition = new Vector2(center.x, closestPos.y);
			lineSize = new Vector2(lineWidth, fontHeight);
			
			//??LINe
			
			startChar = tex.lines[hitLine].startCharIdx;
			if (tex.lines.Count > hitLine + 1) 
				endChar = tex.lines[hitLine + 1].startCharIdx;
			else endChar = indexLimit ;
			
			
			hitChar = closestChar;
			cursorPosition = closestPos;
			// highlight character?
			//cursorPosition.y += top;
			
			// now form the highlight
			
			
			c = chars[hitChar];
			if (!(System.Char.IsWhiteSpace(c)  ))
			{
				FeedCharacters();
			}
			else
			{
				
				wordText = "";
				wordPosition = cursorPosition;
				wordSize = new Vector2(tex.characters[hitChar].charWidth, fontHeight);
			}
			
			
			// find line after?
			FindParagraph();
			// find line after?
			
		}
		
		protected void FindLine(){
			// I can use the margin for a loose bounding box. I can use vertices for tight box.
			
			TextAnchor align;
			align = text.alignment;
			
			if(align == TextAnchor.UpperCenter
			||align == TextAnchor.UpperLeft
			||align == TextAnchor.UpperRight
			)
			{
				top = fourCorners[1].y;
			}
			else if(align == TextAnchor.MiddleCenter
			||align == TextAnchor.		MiddleLeft
			||align == TextAnchor.		MiddleRight
			)
			{
				top = center.y + lineHeight * tex.lines.Count * .5f ;
			}
			else
			{
				top = fourCorners[0].y + lineHeight * tex.lines.Count ;
			}
			top = top - tex.characters[0].cursorPos.y - center.y  - 2f * scaleFactor;
			
			
		}
		
		protected void FindParagraph(){
			// I need to use vertices and compare location
			int vc = hitChar - 1;
			while (vc >= 0 )
			{
				
				c  = chars[vc];
				if (c == '\n' || c == '\r')
				{ break;}
				vc --;
			}
			vc++;
			Vector3 startPos = tex.characters[vc].cursorPos + center ;
			vc = hitChar + 1;
			while (vc < indexLimit )
			{
				
				c  = chars[vc];
				if (c == '\n' || c == '\r')
				{break;}
				vc ++;
			}
			vc--; 
			Vector3 endPos = tex.characters[vc].cursorPos + center ;
			
			endPos.x += tex.characters[vc].charWidth; // bottom right
			endPos.y -= lineHeight; 
			
			// I need to use vertices and compare location
			paragraphSize = (endPos  - startPos);
			paragraphSize.y *= -1;
			paragraphPosition = (startPos + endPos ) / 2f; 
			paragraphPosition.y += top;
		}
			
		protected void FeedCharacters(){
			wordText = "";
			int vc = hitChar - 1;
			int startc ;
			while (vc >= 0 )
			{
				
				c  = chars[vc];
				if (System.Char.IsWhiteSpace(c) )
				{ break;}
				vc --;
			}
			vc++;
			
			startc = vc;
			Vector3 startPos = tex.characters[vc].cursorPos + center ;
			
			
			
			while (vc < indexLimit )
			{
				
				c  = chars[vc];
				if (System.Char.IsWhiteSpace(c)  )
				{ break;}
				wordText += chars[vc] + "";
				
				vc ++;
				
			}	
			vc --;
			
			
			Vector3 endPos = tex.characters[vc].cursorPos + center;
			endPos.x += tex.characters[vc].charWidth; // bottom right
			endPos.y -= fontHeight; // topleft. font already scaled
			
			
			wordSize = (endPos  - startPos);
			wordSize.y *= -1;
			wordPosition = (startPos + endPos ) / 2f;
			
			if (startc < startChar || vc > endChar)
			{
				wordSize.x = lineWidth;
				wordPosition.x = center.x;
			}
			else 
				wordSize.x += 2f;
				
			wordPosition.y += top;
		}
		
		protected float top = 0f;
		protected float left = 0f;
		protected float right = 0f;
		protected float bottom = 0f;
		
		protected override void OnPress(){
						
						
			
		}
	}
}