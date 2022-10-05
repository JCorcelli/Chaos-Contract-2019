using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


namespace ActionSystem
{
	
	public class HumanActions : ActionHandler
	{
		// ActionCallback is subscribed to manager or multiple callback functions
		
		
		
		
		
		/// <summary>
		/// default, similar to idle but as an action
		/// </summary>
		public string holding  = "holding"  ;
		public string petting  = "petting"   ;
		
		public string eating      = "eating"      ;
		public string exploring   = "exploring"   ;
		
		public string moving      = "moving"      ;
		public string napping     = "napping"     ;
		public string sleeping    = "sleeping"    ;
		public string blinking    = "blinking"    ;		
		
		
	}

	
}