using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class WindowHeaderButton : SelectAbstract {
		
		
		public string message = "close"; //min,max,close
		
		public WindowHeaderHook hub;
		protected override void OnEnable(){
			base.OnEnable();
			
			hub = GetComponentInParent<WindowHeaderHook>();
		}
		protected override void OnClick(){
			base.OnClick();
			hub.Send(message);
		}
		
	}
}
