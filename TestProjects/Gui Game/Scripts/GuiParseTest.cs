
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace GuiGame
{

	public class GuiParseTest : MonoBehaviour{ 
	
		public TextAsset text;
		
		protected void Awake(){
			
			if (text == null) {
				Debug.Log("text, where?", gameObject);
				return;
			}
			BParser guiParser = new BParser();
			guiParser.Load(text);
			guiParser.Parse();
			
		}
	}
	
}