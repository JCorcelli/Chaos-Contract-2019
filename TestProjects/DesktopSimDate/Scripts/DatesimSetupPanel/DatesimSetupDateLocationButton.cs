using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupDateLocationButton : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		
		public RectTransform rectTransform;
		protected RawImage image;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			rectTransform = GetComponent<RectTransform>();
			image = GetComponent<RawImage>();
		}
		protected bool selected = false;
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			if (!selected && SelectGlobal.selected == gameObject)
			{
				OnPress();
			}
			
		}
		
		
		public int relationRequired = 1;
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			selected = (hub.location == name);
				
			
			if (relationRequired > hub.access)
			{
				if ( selected )
				{
					
					hub.location = "None";
					selected = false;
				}
				gameObject.SetActive(false);
			}
			else
				gameObject.SetActive(true);
			
			
		}
		
		protected override void OnPress()
		{
			hub.location = name;
			hub.Preview();
		}
		
		
		
		
		

	}
}