using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;


namespace Datesim
{
	public class DatesimOnStart : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public bool started = false;
		protected override void OnEnable(){
			base.OnEnable();
			
			vars.CleanApp();
			started = true; 
			// connect node to ??? big game node?
		}
		
		
		
		
		
		
		

	}
}