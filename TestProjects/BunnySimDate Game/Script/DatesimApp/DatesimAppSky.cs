using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppSky : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		
		protected override void OnEnable( ){
			base.OnEnable();
			child = transform.GetChild(0).gameObject;
			OnChange();
		}
		
		protected override void OnChange(){
			
			if (vars.sky == "") vars.sky = "None";
			
			if (vars.sky.ToLower() == gameObject.name.ToLower()) 
				child.SetActive(true);
			else 
				child.SetActive(false);
		}

	}
}