using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SelectionSystem;

namespace DialogueSystem
{
	public class TextBoxInfo1 : SelectAbstract {

		protected string currentWordText = "";
		
		protected Text text;
		//new public Transform rectTransform;
		public RectTransform highlight ;
		public RectTransform rectTransform;
		
		float wordLength = 0f;
		float wordPosition = 0f;
		int hitChar  = 0;
		protected override void OnEnable () { 
			base.OnEnable();
			// child is highlight
			rectTransform = GetComponent<RectTransform>();
			if (highlight == null)
				Debug.LogError("make a highlight already", gameObject);
			
			text = GetComponent<Text>();

		}
		protected char[] chars;
		protected int endChar = 0;
		protected int startChar = 0;
		protected TextGenerator tex;
		protected Vector2 center;
			
		protected Vector3 lastMousePos = new Vector3();
		protected override void OnUpdate( ) {
			base.OnUpdate();
			if (!isHovered) return;
			Vector3 mousePos = Input.mousePosition;
			
			tex = text.cachedTextGenerator;
			if (tex == null ||  tex.characters.Count < 1 ) return;
			
			if (text.text.Length < 1 ) return;
			
			chars = 
				text.text.ToCharArray();
				
			
				
			wordLength = 0f;
			wordPosition = 0f;
			
			// this works to fix line spacing
			float lineHeight = tex.lines[0].height ;
			float fontHeight = lineHeight / text.lineSpacing;

			Vector3[] fourCorners = new Vector3[4];
			rectTransform.GetWorldCorners(fourCorners);
			
			center = new Vector2(
			(fourCorners[0].x + fourCorners[2].x) /2f,
			(fourCorners[0].y + fourCorners[2].y) /2f
			);
			
			TextAnchor align;
			align = text.alignment;
			float top ; // for line
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
				top = center.y;
				top += lineHeight * tex.lines.Count * .5f;
			}
			else
			{
				top = fourCorners[0].y + lineHeight * tex.lines.Count;
			}
			float lineWidth;
			//float lineHeight;
			
			
			int fin = 0;
			startChar = 0;
			// the y position on cursor is not great, how do I find the right one?
			for (int i = 0; i < tex.lines.Count; i++)
			{
				
				fin = i; 
				//startChar = tex.lines[i].startCharIdx;
				lineHeight = tex.lines[fin].height;
				top -= lineHeight;
				
				if (top < mousePos.y) 
					break;
			}
			startChar = tex.lines[fin].startCharIdx;
			
			// this sets the exact position (pretty accurate)
			top += lineHeight - (fontHeight / 2f) ; 
			Vector3 linePosition = new Vector3(center.x, top, 0f);
			lineWidth = rectTransform.rect.width;
			//lineHeight = tex.lines[fin].height;
			
			
			bool highlightLine = false;
			if (highlightLine)
			{
				highlight.position = linePosition;
				highlight.sizeDelta =  new Vector2(lineWidth, fontHeight);
				return;
			}
			
			// for word
			if (tex.lines.Count > fin + 1) 
				endChar = tex.lines[fin + 1].startCharIdx;
			else endChar = tex.characters.Count ;
			float left; // for char
			
			left = center.x + tex.characters[startChar].cursorPos.x;
			
			
			float right; // for char

			right = center.x + tex.characters[endChar - 1].cursorPos.x + tex.characters[endChar - 1].charWidth; 
			
			if (mousePos.x < left)
			{
				hitChar = startChar;
				// the margins?
				//
			}
			else if (mousePos.x > right)
			{
				hitChar = endChar - 1 ;
				// the margins?
				// 
			}
			
			else
			{
				hitChar = startChar;
				for (int i = startChar; i < endChar; i++)
				{
					left = center.x + tex.characters[i].cursorPos.x + tex.characters[i].charWidth;
					hitChar = i;
					if (mousePos.x < left)
					{ 
				
						break;
					}
				}
			}
			// highlight character?
			
			
			// now form the highlight
			
			
			if (chars.Length <= hitChar) return;
			
			if (chars[hitChar] != ' '
			&& chars[hitChar] != '\t'
			&& chars[hitChar] != '\n'
			&& chars[hitChar] != '<'
			&& chars[hitChar] != '>'
			&& chars[hitChar] != ',')
			{
				currentWordText = "";
				FeedCharacters();
			}
			else
			{
				left = center.x + tex.characters[hitChar].cursorPos.x + tex.characters[hitChar].charWidth / 2f ;
			
				wordPosition =  left;
				wordLength = tex.characters[hitChar].charWidth;
			}
			
			float scaleFactor = rectTransform.lossyScale.y;
			
			highlight.sizeDelta =  new Vector2( wordLength, fontHeight) / scaleFactor;
			
			highlight.position = new Vector3( wordPosition , top, 0f );
			
			
		}
		
		protected int currentWordPos = 0;
		protected void FeedCharacters(){
			int vc = hitChar;
			while (vc > 0 )
			{
				
				vc --;
				if (chars[vc] == ' '
				|| chars[vc] == '\t'
				|| chars[vc] == '\n'
				|| chars[vc] == '<'
				|| chars[vc] == '>'
				|| chars[vc] == ',')
				{vc++; break;}
			}
			currentWordPos = vc;
			
			float startPos = center.x + tex.characters[vc].cursorPos.x  ;
			
			float endPos ;
			//vc < chars.Length && 
			while (vc < endChar)
			{
				
				if (chars[vc] == ' '
				|| chars[vc] == '\t'
				|| chars[vc] == '\n'
				|| chars[vc] == '<'
				|| chars[vc] == '>'
				|| chars[vc] == ',')
				{ break;}
				currentWordText += chars[vc] + "";
				
				vc ++;
				
			}	
			vc --;
			
			if (vc >= endChar) vc = endChar - 1;
			endPos = center.x + tex.characters[vc].cursorPos.x + tex.characters[vc].charWidth;
			
			wordLength = (endPos  - startPos);
			wordPosition = ( startPos + endPos ) / 2f;
		}
		
		
		protected override void OnPress(){
						
						
			
		}
	}
}