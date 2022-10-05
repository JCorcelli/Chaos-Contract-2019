using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimDesktopDateStartWaitMode : DatesimDesktopConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public Button button;
		public GameObject hiddenObject;
		protected override void OnEnable() {
			base.OnEnable();
			
			button = GetComponent<Button>();
			OnChange();
		}
		protected override void OnChange() {
			if (vars == null) return;
			
			
				
		}
		
		
		protected override void OnPress() {
			
			base.OnPress(); // maybe flash a bit
			vars.mode = "wait mode";

			vars.StartDate();
			
			
		}
		

	}
}