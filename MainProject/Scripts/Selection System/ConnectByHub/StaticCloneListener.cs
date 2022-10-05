using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace SelectionSystem
{
	public class StaticCloneListener : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			Connect();
		}
		public bool copyFlag = false;
		protected override void OnEnable( ){
			if(copyFlag) 
			{
				GameObject.Destroy(this);
				return;
			}
			copyFlag = true;
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