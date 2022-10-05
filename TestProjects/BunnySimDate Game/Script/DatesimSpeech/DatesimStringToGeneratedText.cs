using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Datesim 
{
	[ExecuteInEditMode]
	public class DatesimStringToGeneratedText : MonoBehaviour {
		public float point = 20f;
		public TextAsset textFile;
		protected string text;
		public RectTransform rectTransform;
		public RectTransform currentLine;
		public RectTransform currentWord;
		public Text currentWordText ;
		public Text t ;
		
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
			t = GetComponent<Text>();
		}
		protected void Update(){
			
			if (Application.isPlaying) 
				PlayUpdate();
			else
			EditorUpdate();
		}
		
		protected void PlayUpdate(){
			// line \n is a paragraph
				// words
		}
		protected void EditorUpdate(){
			
			

			if (textFile == null) return;
			text = textFile.ToString();
			//if (text != newText) text = newText;
			if (text == "") return;
			t.text = text;
			
			
			
		}
		
		
	}
}