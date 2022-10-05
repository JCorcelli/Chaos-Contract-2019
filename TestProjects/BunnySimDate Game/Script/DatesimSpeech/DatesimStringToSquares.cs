using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Datesim 
{
	[ExecuteInEditMode]
	public class DatesimStringToSquares : MonoBehaviour {
		public RectTransform wordCopy;
		public RectTransform squareCopy;
		public RectTransform spaceCopy;
		public RectTransform tabCopy;
		public RectTransform lineCopy;
		public string text = "";
		public RectTransform rectTransform;
		public RectTransform currentLine;
		public RectTransform currentWord;
		public Text currentWordText ;
		
		protected RectTransform sq;
		protected char s;
		
		
		protected void OnEnable() {
			rectTransform = GetComponent<RectTransform>();
			
		}
		protected void Update(){
			if (squareCopy == null) return;
			if (spaceCopy == null) return;
			if (tabCopy == null) return;
			if (lineCopy == null) return;
			while (rectTransform.childCount > 0)
			{
				DestroyImmediate(rectTransform.GetChild(0).gameObject);
			}
			
			

			if (text == "") return;
			AddLine();
			lineStarter = false;
			//AddWord();
			//  a string replace first. for those characters I want to interpret.
			// then later, the images I want to add
			
	  
			char[] tt = 
				text
				  .Replace("\\n", "\n")
				  .Replace("\\t", "\t")
				  .ToCharArray();
			
			for (int i  = 0; i < tt.Length  ; i ++ )
			{
				
				s = tt[i];
				if (s == '\n')
					AddLine();
				else if (s == ' ')
				{
					AddSpace();
				}
				else if (s == '\t')
					AddTab();
					
				else
				{
					CombineText(); //AddChar
					FeedCharacter();
				}
				Wraps();
			}
			
				
		}
		
		protected bool currentWordIsSpace = true;
		protected void AddLine(){
			
			RectTransform line = Instantiate(lineCopy) as RectTransform;
			line.SetParent( rectTransform, false );
			currentLine = line;
			totalLength = 0;
			AddWord();
		}
		
		protected bool lineStarter = false;
		protected void AddLineWrap(){
			// I think this fixes it. but I didn't compile test.
			RectTransform line = Instantiate(lineCopy) as RectTransform;
			line.SetParent( rectTransform, false );
			currentLine = line;
			totalLength = 0;
			if (currentWordIsSpace || wordLength > rectTransform.rect.width / 2f)
			{
				AddWord();
				lineStarter = true;
			}
		}
		protected void AddSpace(){
			if (lineStarter) return;
			if (!currentWordIsSpace)  AddWord();
			sq = Instantiate(spaceCopy) as RectTransform;
			sq.SetParent( currentWord, false );
		
			totalLength += 5 ;
			
			
			
		}
		protected void AddTab(){
			
			if (!currentWordIsSpace)  AddWord();
			sq = Instantiate(tabCopy) as RectTransform;
			sq.SetParent( currentWord, false );
			totalLength += 15 ;
			
		}
		public float totalLength = 0;
		public float wordLength = 0;
		
		protected void AddWord(){
				
			currentWord= Instantiate(wordCopy) as RectTransform;
			currentWord.SetParent(currentLine, false);
			
			
			currentWordText = currentWord.GetComponent<Text>();
			currentWordIsSpace = true;
			wordLength = 0;
		}
		protected void CombineText(){
			if (currentWordIsSpace && currentWord.childCount > 0) AddWord();
			currentWordIsSpace = false;
			lineStarter = false;
			Text t = currentWordText;
			t.text +=  s + ""; // I'll have to check if a character is out of bounds...
			
			
			
			
		}
			
		protected void AddChar(){
			currentWordIsSpace = false;
			lineStarter = false;
			sq = Instantiate(squareCopy) as RectTransform;
			
			Text t = sq.GetComponent<Text>();
			
			t.text = s + ""; // I'll have to check if a character is out of bounds...
			
			
			// now add the thing?
			
			// i should check word length
			
			
			sq.SetParent( currentWord, false );
				
			
		}
		
		
		protected void FeedCharacter(){
			
			
			Text t = currentWordText;
			Font font = t.font; //text is my UI text
			CharacterInfo characterInfo = new CharacterInfo();

	
			font.RequestCharactersInTexture(s.ToString(), t.fontSize, t.fontStyle);
			font.GetCharacterInfo(s, out characterInfo, t.fontSize);
			
			float advance = characterInfo.advance;
			totalLength += advance;
			wordLength += advance;
		}
		protected void Wraps(){
			// prepare line wrap
			
			// scale it somehow, it's getting too wide?
			// might want +10 or + "-" length
			
			
			if (totalLength + 10  < rectTransform.rect.width ) return ;
			
			// true
			AddLineWrap(); // changes line
			currentWord.SetParent(currentLine, false);
			
		}
	}
}