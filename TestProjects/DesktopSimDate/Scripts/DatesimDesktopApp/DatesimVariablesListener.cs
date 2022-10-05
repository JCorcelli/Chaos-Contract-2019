using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimVariablesListener : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		public DatesimVariables vars;
		public DatesimHub hub;
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			Connect();
		}
		protected override void OnEnable( ){
			base.OnEnable();
			vars = GetComponent<DatesimVariables>();
			
			
			
			hub = GetComponent<DatesimHub>();
			CheckConnected();
		}
		
		
		protected override void OnConnect(object ob) {
		}
		
		
		public override void OnChange() {
			// behavior
			if (onChange != null) onChange();
		}
		

	}
}