using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupDateEventButton : DatesimSetupDateConnect {
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
			SetEventDay();
		}
		
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			
			
		}
		
		protected override void OnEnter(){
			
			if (event_day)image.color = Color.red;
		}
		protected override void OnExit(){
			
			if (event_day)image.color = Color.black;
		}
		
		protected bool event_day = false;
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			if (event_day == hub.event_day) return;
			
			event_day = hub.event_day;
			SetEventDay();
			
		}
		protected void SetEventDay()
		{
		
			if (event_day)
			{
				if (marks.Length > 0)
				{
					int used = (int)Random.Range(0, marks.Length -1 );
					image.sprite = marks[used];
				}
				image.color = Color.black;
			}
			else
			{
				image.color = Color.clear;
				
			}
		}
		protected override void OnClick()
		{
			
			if (hub.vars.relation < hub.eventRelation) return;
			
			hub.event_day = !hub.event_day;
			hub.weekday = hub.weekdayOptions[hub.intday];
			hub.Preview();
		}
		
		
		
		
		

	}
}