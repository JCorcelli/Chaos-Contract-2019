using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Datesim 
{
	[ExecuteInEditMode]
	public class DatesimStringToGeneratedText1 : MonoBehaviour {
		public RectTransform prefabCopy;
		public RectTransform wordCopy;
		public RectTransform spaceCopy;
		public RectTransform tabCopy;
		public RectTransform lineCopy;
		public float point = 20f;
		public TextAsset textFile;
		protected string text;
		public RectTransform rectTransform;
		public RectTransform currentLine;
		public RectTransform currentWord;
		public Text currentWordText ;
		
		protected RectTransform sq;
		protected char s;
		
		
		protected void OnEnable() {
			rectTransform = GetComponent<RectTransform>();
			
			// I could set all the variables like so
			//lineCopy.sizeDelta = new Vector2 ( 1, point);
			
			/*
			linecopyheight = point
			wordheight = point
			
			
			*/
		}
		protected void Update(){
			
			if (Application.isPlaying) 
				PlayUpdate();
			else if (updateNow)
			{
				updateNow = false;
				EditorUpdate();
			}
		}
		public bool updateNow = false;
		protected void PlayUpdate(){
			// line \n is a paragraph
				// words
		}
		protected void EditorUpdate(){
			if (prefabCopy == null) return;
			if (wordCopy == null) return;
			if (spaceCopy == null) return;
			if (tabCopy == null) return;
			if (lineCopy == null) return;
			
			while (rectTransform.childCount > 0)
			{
				DestroyImmediate(rectTransform.GetChild(0).gameObject);
			}
			
			

			if (textFile == null) return;
			text = textFile.ToString();
			//if (text != newText) text = newText;
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
				// prefab test
				if (s == '}')
				{
					gathering = false;
					
					AddPrefab();
					
				}
				else if (gathering)
				{
					prefabName += s + "";
					
					continue;
				}
				else if (s == '{')
				{
					gathering = true;
					prefabName = "";
					continue;
				}
				// special characters
				else if (s == '\n')
				{
					AddLine();
					continue;
				}
				else if (s == ' ')
				{
					AddSpace();
				}
				else if (s == '\t')
					AddTab();
					
				// normal text
				else
				{
					CombineText(); //AddChar
					FeedCharacter();
				}
				Wraps();
			}
			
				
		}
		
		
		protected bool gathering = false;
		public string prefabName = "";
		
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
		
			totalLength += spaceCopy.rect.width ;
			
			
			
		}
		protected void AddTab(){
			
			if (!currentWordIsSpace)  AddWord();
			sq = Instantiate(tabCopy) as RectTransform;
			sq.SetParent( currentWord, false );
			totalLength += tabCopy.rect.width ;
			
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
		protected void AddPrefab(){
			
			RectTransform rt = UnityEngine.Resources.Load("Datesim/"+prefabName, typeof(RectTransform)) as RectTransform;
			
			if (rt == null)return;
			sq = Instantiate(rt) as RectTransform;
			
			//sq.sizeDelta = new Vector2(wordCopy.height, wordCopy.height);
			
			LayoutElement le = sq.GetComponent<LayoutElement>();
			
			le.ignoreLayout = false;
			le.preferredWidth = point;
			le.preferredHeight = point;
			RectTransform sqt = sq.GetChild(0) as RectTransform;
			float size = point/sqt.rect.height; // puts the image inline
			Vector3 newScale = new Vector3(size,size,1f);
			sqt.localScale = newScale;
			
			sq.SetParent( currentLine, false );
			currentWord = sq;
			Wraps();
			totalLength += point ;
			AddWord();
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