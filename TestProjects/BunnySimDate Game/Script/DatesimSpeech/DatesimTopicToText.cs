using UnityEngine;

using UnityEngine.UI;
using System.Collections;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimTopicToText : DatesimAppConnectLite {
		
		public string[] lines = new string[]{};
		public Text text;
		public TextAsset textFile;
		
		protected override void OnEnable(){
			base.OnEnable();
			
			
			if (text == null) 
				text = GetComponent<Text>();
			OnChangeX();
		}
		protected void ImportText()
		{
			
			if (lines.Length > 0) return;
			if (textFile == null) return;
			
			string[] separatingChars = { "\r\n###########\r\n" };  
			string s = textFile.ToString();
			
			lines = s.Split(separatingChars, System.StringSplitOptions.None);
			
			
		}
		
		public string topic = "";
		
		public int textStage = 1;
		
		protected void OnChangeX(){
			if (vars == null) return;
			topic = vars.topic;
			
			ImportText();
			
			foreach (string s in lines)
			{
				
				if (s.ToLower().StartsWith(topic.ToLower()) )
				{
					
					string[] current= s.Split(new string[]{"*****"}, System.StringSplitOptions.RemoveEmptyEntries);
					
					if (current.Length <= textStage) text.text = "";
					else
						text.text = current[textStage].Trim();
					return;
				}
			}
		}
	}
}