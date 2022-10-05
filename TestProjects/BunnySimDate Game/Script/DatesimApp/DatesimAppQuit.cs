using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppQuit : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		
		
		protected override void OnEnable() {
			base.OnEnable();
			
			
		}
		
		
		protected override void OnPress() {
			
			base.OnPress();
			
			vars.QuitApp();
			
			
			
		}
		

	}
}