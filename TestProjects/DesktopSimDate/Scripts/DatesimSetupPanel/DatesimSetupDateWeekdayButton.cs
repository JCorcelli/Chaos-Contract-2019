using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupDateWeekdayButton : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		
		public RectTransform rectTransform;
		protected RawImage image;
		
		public int incrementBy = 1;
		public int intday = 0; // monday
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			rectTransform = GetComponent<RectTransform>();
			image = GetComponent<RawImage>();
		}
		
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			
			
		}
		
		public int access = 1;
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			
			if ((int)hub.access == access) return;
			
			if (hub.event_day) 
			{
				return;
			}
			
		}
		
		
		public void SetDay(){
			
			if (hub.event_day) 
			{
				return;
			}
			access = hub.access;
			intday = hub.intday;
			intday += incrementBy;
			hub.intday = intday = (int)Mathf.Repeat(intday, hub.weekdayAccess[access]);
			
			if (hub.weekdayOptions.Length  > 0) hub.weekday = hub.weekdayOptions[intday];
			
			
		}
		protected override void OnPress()
		{
			SetDay();
			hub.Preview();
		}
		
		
		
		
		

	}
}