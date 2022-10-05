using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppLock : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		public GameObject hiddenObject;
		
		protected override void OnEnable() {
			base.OnEnable();
			OnChange();
		}
		protected override void OnChange() {
			if (vars.mode == "realtime") 
			{
				hiddenObject.SetActive(true);
			}
			
			else
			{
				hiddenObject.SetActive(false);
			}
			
				
		}
		
		protected override void OnPress() {
			
			// I should play animation
			
		}
		

	}
}