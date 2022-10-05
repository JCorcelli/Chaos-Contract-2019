using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppAddResponse : DatesimAppConnectLite {
		// This is intended for big applications that need to coordinate actions
		
		public int message = 1;
		protected override void OnChange() {
			
			if (vars.stage == message)
				vars.Response();
			
		}
		

	}
}