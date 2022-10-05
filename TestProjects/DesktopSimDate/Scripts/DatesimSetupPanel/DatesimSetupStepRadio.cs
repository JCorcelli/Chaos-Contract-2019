using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupStepRadio : DatesimSetupDateConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		public int step = 1;
		
		public bool multistep = false;
		public int lessThan = 4;
		public int greaterThan = 0;
		
		protected override void OnEnable( ){
			base.OnEnable();
			child = transform.GetChild(0).gameObject;
			Set();
		}
		protected override void OnChange(){Set();}
		protected void Set(){
			
			if (hub.step == step) 
				child.SetActive(true);
			else if (multistep
			&& hub.step < lessThan
			&& hub.step > greaterThan)
			{
				child.SetActive(true);
			}
			else child.SetActive(false);
			
		}
		protected override void OnPress(){
			hub.step = step;
			hub.Preview();
		}

	}
}