using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppStart : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public GameObject hiddenObject;
		
		
		public bool available{ get{return (vars.proximity && !vars.app_on);} set{}}
		protected override void OnEnable() {
			base.OnEnable();
			OnChange();
		}
		protected override void OnChange() {
			
			if (available)
			{
				hiddenObject.SetActive(true);
			}
			
			else
			{
				hiddenObject.SetActive(false);
			}
			
				
		}
		
		protected override void OnUpdate() {
			base.OnUpdate();
			if (pressed) return;
			
			if (available && Input.GetButtonDown("use"))  OnPress();
		}
		protected override void OnPress() {
			
			base.OnPress();
			if (vars.app_on) return;
			vars.dateZone = true;
			vars.StartApp();
			
		}
		

	}
}