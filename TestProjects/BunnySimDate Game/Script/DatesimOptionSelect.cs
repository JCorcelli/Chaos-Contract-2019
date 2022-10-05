using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimOptionSelect : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		protected override void OnEnable(){
			base.OnEnable();
			buttonName = "mouse 1";
		}
		public DatesimDelegate onClick;
		protected override void OnClick() {
			
			base.OnClick();
			
			if (onClick != null) onClick();
			
			
		}
		

	}
}