using UnityEngine;

using UnityEngine.UI;
using System.Collections;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimRandomText : UpdateBehaviour {
		
		public string[] lines;
		public Text text;
		public TextAsset textFile;
		
		protected override void OnEnable(){
			base.OnEnable();
			
			if (text == null) 
				text = GetComponent<Text>();
			if (lines.Length == 0) 
			{
				if (textFile == null) return;
				ImportText();
			}
			if (lines.Length == 0) 
				return;
			
			float rand = Random.Range(0f, (float)lines.Length - 1);
			int choice = (int)Mathf.Round(rand);
			
			text.text = lines[choice];
		}
		protected void ImportText()
		{
			string[] separatingChars = { "\r\n>>>\r\n" };  
			string s = textFile.ToString();
			lines = s.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
		}
	}
}