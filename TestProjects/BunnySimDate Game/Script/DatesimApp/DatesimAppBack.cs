using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility;
using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppBack : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		public GameObject hiddenObject;
		protected Canvas canvas;
		protected override void OnEnable() {
			base.OnEnable();
			if (canvas == null) canvas = AddCanvas.Util(hiddenObject.transform);
			else
				AddCanvas.Fix(canvas);
			OnChange();
		}
		protected override void OnChange() {
			if (vars.stage > 1 && vars.stage < 5)
			{
				canvas.enabled = true;
			}
			
			else
			{
				canvas.enabled = false;
			}
			
				
		}
		
		protected override void OnPress() {
			
			base.OnPress();
			vars.Back();
			
			
		}
		

	}
}