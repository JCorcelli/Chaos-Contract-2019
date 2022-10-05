using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppWeekday : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		protected string current = "init";
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
				t.gameObject.SetActive(false);
			}
			
			OnChange();
		}
		
		protected override void OnChange(){
			if (vars.weekday == "") return;
			
			if (vars.event_day)
			{
				current = "vacation";
				transform.Find("Vacation").gameObject.SetActive(true);
				return;
			}
			
			if (current == vars.weekday) return;
			
			// normal
			foreach (Transform t in transform)
			{
				if (vars.weekday.ToLower() == t.name.ToLower())
				{
					child.SetActive(false);
					child = t.gameObject;
					child.SetActive(true);
					current = vars.weekday;
					return;
				}
			}
			// I shouldn't be here
		}

	}
}