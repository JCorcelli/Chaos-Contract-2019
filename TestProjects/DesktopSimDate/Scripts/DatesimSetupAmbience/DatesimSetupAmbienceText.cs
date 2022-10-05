﻿using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceText : DatesimSetupAmbienceConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		public Text text;
		
		
		
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			
		
			if (text == null)
			text = GetComponent<Text>();
		
		}
		
		
		
		
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			
			text.text = ambienceHub.totalAmbience + " / 50";
		}
		
		
		
		
		
		
		

	}
}