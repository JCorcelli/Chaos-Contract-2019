using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceClearButton : DatesimSetupAmbienceConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		protected override void OnClick()
		{
			ambienceHub.ClearAll();
			ambienceHub.OnChange();// may be unnecessary
		}
		
		
		
		
		

	}
}