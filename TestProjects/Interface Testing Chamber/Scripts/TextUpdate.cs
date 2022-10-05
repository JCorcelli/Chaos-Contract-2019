using UnityEngine;
using System.Collections;
using UnityEngine.Events;

using UnityEngine.UI;

namespace Utility
{
	public class TextUpdate : UpdateBehaviour {

		
		// this'll be great for UI debug / banner text
		
		protected Text t;
		

		protected void Awake() { 
			t = GetComponent<Text>(); 
		}
		
		protected virtual void SetText(string newText) { 
			t.text = newText;
		}
		
	}

}