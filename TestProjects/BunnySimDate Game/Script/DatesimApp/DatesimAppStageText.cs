using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppStageText : DatesimAppConnectText {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public int displayStage = 0;
		
		protected override void OnChange() {
			base.OnChange();
			
			displayStage = vars.stage;
			if (displayStage == 0) text.text = "Stage : off";
			else if (displayStage < 5)
				text.text = "Stage : " + displayStage + " of 4";
			else
				text.text = "Stage : Complete";
		}
		
		
		

	}
}