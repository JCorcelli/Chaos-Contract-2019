using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppOnscreenMessage : DatesimAppConnectLite {
		// This is intended for big applications that need to coordinate actions
		
		
		public Text text;
		protected string useString = "use (e) or click";
		protected string zoneString = "(space) enter zone";
		protected string cancelString = "(space) leave";
		protected string continueString = "(space) continue";
		protected override void OnEnable() {
			base.OnEnable();
			text = GetComponent<Text>();
			OnChange();
		}
		protected override void OnChange() {
			if (!vars.proximity) 
				text.text = "";
			else if (vars.dateZone)
			{
				text.text = cancelString;
			}
			else if (vars.app_on)
			{
				text.text = continueString;
			}
			else if (vars.inZone)
			{
				text.text = "";
			}
			else // zone
			{
				text.text = zoneString;
			}
			
			text.SetLayoutDirty();
		}
		
		

	}
}