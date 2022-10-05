using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace SelectionSystem
{
	public abstract class StaticHookExample : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		
		
		protected string targetName = "";
		protected  StaticHubConnect target;
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			connected = (target != null);
			
			if (connected) return;
			//if (_hook) 
			Ping();
		}
		protected override void OnEnable( ){
			base.OnEnable();
			
			CheckConnected();
			
			
		}
		
		protected override void OnConnect(object ob) {
			// for hooks
			if (ob.GetType() == typeof(StaticHubConnect) )
			{
				target = ((StaticHubConnect)ob);
				target.connected = connected = true;
				
				target.onChange += OnChange;
				return;
				
			}
		}
		
		
		public override void OnChange() {
			// behavior
		}

	}
}