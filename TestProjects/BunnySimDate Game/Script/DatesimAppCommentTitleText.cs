using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppCommentTitleText : DatesimAppConnectText {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public string sTitle = "";
		
		protected override void OnChange() {
			base.OnChange();
			
			sTitle = vars.pcomment;
			text.text = sTitle;
		}
		
		
		

	}
}