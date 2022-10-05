using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppLocation : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		protected override void OnEnable( ){
			base.OnEnable();
			child = transform.GetChild(0).gameObject;
			OnChange();
		}
		
		protected override void OnChange(){
			if (vars.location.ToLower() == gameObject.name.ToLower()) 
				child.SetActive(true);
			else 
				child.SetActive(false);
		}

	}
}