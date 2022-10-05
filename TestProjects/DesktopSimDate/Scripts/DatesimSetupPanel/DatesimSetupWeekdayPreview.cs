using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupWeekdayPreview : DatesimSetupDateConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		public string current = "init";
		protected override void OnEnable( ){
			base.OnEnable();
			child = transform.GetChild(0).gameObject;
			if (child == null) 
			{
				Debug.LogError("no child", gameObject);
				return;
			}
			foreach (Transform t in transform)
			{
				if (t.gameObject != child)
				t.gameObject.SetActive(false);
			}
			Set();
		}
		protected override void OnChange(){Set();}
		protected void Set(){
			if (hub.weekday == "") 
				return;
			
			
			if (current == hub.weekday) return;
			
			// normal
			foreach (Transform t in transform)
			{
				if (hub.weekday.ToLower() == t.name.ToLower())
				{
					child.SetActive(false);
					child = t.gameObject;
					child.SetActive(true);
					current = hub.weekday;
					return;
				}
			}
			
		}

	}
}