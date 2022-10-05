using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace SelectionSystem
{
	public abstract class StaticListenerExample : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			Connect();
		}
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			
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