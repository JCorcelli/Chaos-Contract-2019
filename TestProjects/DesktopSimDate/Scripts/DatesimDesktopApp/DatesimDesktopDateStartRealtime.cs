using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimDesktopDateStartRealtime : DatesimDesktopConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public Button button;
		public GameObject hiddenObject;
		public GameObject hasPlayedObject;
		public GameObject hasNotPlayedObject;
		protected override void OnEnable() {
			base.OnEnable();
			
			button = GetComponent<Button>();
			OnChange();
		}
		protected override void OnChange() {
			if (vars == null) return;
			
			
				
		}
		
		
		protected override void OnPress() {
			if (!vars.has_played) return;
			base.OnPress(); // maybe flash a bit
			vars.mode = "realtime";

			vars.StartDate();
			
			
		}
		

	}
}