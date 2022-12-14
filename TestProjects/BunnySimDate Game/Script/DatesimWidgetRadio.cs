using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;


namespace Datesim
{
	public class DatesimWidgetRadio : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public int keyMessage = 1;
		public GameObject widgetObject;
		
		protected override void OnEnable( ){
			
			base.OnEnable();
			if (widgetObject == null)
				widgetObject = gameObject;
			Connect();
		}
		
		
		
		
		protected override void OnChange() {
			
			
			//if (!gameObject.activeInHierarchy) return;
			
			if (vars.stage == keyMessage)
			{
				widgetObject.SetActive(true);
			}
			else
			{
				widgetObject.SetActive(false);
			}
				
					
			
				
		}
		
		
		

	}
}