using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupSkyPreview : DatesimSetupDateConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		
		protected override void OnEnable( ){
			base.OnEnable();
			child = transform.GetChild(0).gameObject;
			Set();
		}
		
		protected override void OnChange(){Set();}
		protected void Set(){
			
			if (hub.sky == "") hub.sky = "None";
			
			if (hub.sky.ToLower() == gameObject.name.ToLower()) 
				child.SetActive(true);
			else 
				child.SetActive(false);
		}

	}
}