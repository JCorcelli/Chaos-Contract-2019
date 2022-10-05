using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupDateClearAllButton : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		public Sprite[] marks;
		public RectTransform rectTransform;
		protected Image image;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			rectTransform = GetComponent<RectTransform>();
			image = GetComponent<Image>();
		}
		
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			
			
		}
		
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			
		}
		
		protected override void OnClick()
		{
			
			hub.ClearAll();
			hub.Preview();
		}
		
		
		
		
		

	}
}