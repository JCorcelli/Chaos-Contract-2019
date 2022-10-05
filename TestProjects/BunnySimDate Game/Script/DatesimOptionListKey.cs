using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimOptionListKey : DatesimOptionKey {
		// This is intended for big applications that need to coordinate actions
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			buttonName = transform.GetSiblingIndex() + 1 + "";
		}
		
		

	}
}