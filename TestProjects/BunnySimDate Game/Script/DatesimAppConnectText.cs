using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppConnectText : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public Text text;
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (text == null) text = GetComponentInParent<Text>();
			if (text == null) {
				Debug.Log("no text, this broke", gameObject);
				return; 
			}
			
			OnChange();
		}
		

	}
}