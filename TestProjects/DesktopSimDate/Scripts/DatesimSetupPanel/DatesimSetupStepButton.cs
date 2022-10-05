using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupStepButton : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		
		
		public int incrementBy = 1;
		
		
		
		protected override void OnPress()
		{
			
			hub.step += incrementBy;
			
			hub.Preview();
		}
		
		
		
		
		

	}
}