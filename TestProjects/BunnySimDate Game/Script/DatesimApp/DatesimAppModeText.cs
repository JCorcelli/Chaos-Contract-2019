using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppModeText : DatesimAppConnectText {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public string displayMode = "";
		
		protected override void OnChange() {
			base.OnChange();
			
			displayMode = vars.mode;
			text.text = displayMode;
		}
		
		
		

	}
}