using UnityEngine;

using UnityEngine.UI;
using System.Collections;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimOptionChangeTopic : DatesimAppConnectLite {
		
		public string[] lines;
		
		public TextAsset textFile;
		
		public int channel = (int)DatesimHub.Channel.Option;
		public int message = (int)DatesimHub.OptionEnum.ChangeTopic;
		
		protected override void OnEnable(){
			base.OnEnable();
			NewTopic();
		}
		
		
		
		protected override void OnChange() {
			
				
			if (!vars.newTopic) return;
			
			vars.newTopic = false;
			NewTopic();
			
			
			
		}
		protected void NewTopic() {
			
			if (lines.Length == 0) 
			{
				if (textFile == null) return;
				ImportText();
			}
			if (lines.Length == 0) 
				return;
			
			float rand = Random.Range(0f, (float)lines.Length - 1);
			int choice = (int)Mathf.Round(rand);
			vars.topic = lines[choice];
			vars.OnChange();
			
		}
		protected void ImportText()
		{
			string[] separatingChars = { ">>>" };  
			string s = textFile.ToString();
			lines = s.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
		}
	}
}