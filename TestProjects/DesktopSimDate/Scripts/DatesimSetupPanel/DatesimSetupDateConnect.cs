using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupDateConnect : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public DatesimSetupDateHub hub;
		
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (hub == null)
			{
				hub = GetComponentInParent<DatesimSetupDateHub>();
			}
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			
			hub.onChange -= OnChange;
			hub.onChange += OnChange;
			
			
			
			
		}
		
		protected virtual void OnChange() {
			// if "" == "" don't change it
		}
		
		
		
		
		
		
		
		protected override void OnPress() {
			
			// if "" == "" don't change it
			
		}
		

	}
}