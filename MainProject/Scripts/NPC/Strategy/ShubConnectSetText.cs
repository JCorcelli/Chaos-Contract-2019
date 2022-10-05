using UnityEngine;
using UnityEngine.UI;

namespace NPC.Strategy
{
	public class ShubConnectSetText : ShubConnect {
		public Text text;

		
		protected void Awake() {
			text = GetComponent<Text>();
		}
		protected override void OnStart(){
			SetText();
		}
		protected override void OnStop(){
			
			SetText();
			
		}
		
		protected void SetText() {
			string newText = "";
			foreach (string strat in shub.list)
				newText += strat + "\n";
			
			if (newText == "") return;
			newText = newText.Substring(0, newText.Length -1);
			
			text.text = newText;
		}
		
		
	}
}