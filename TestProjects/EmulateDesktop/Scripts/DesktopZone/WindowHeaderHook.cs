using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class WindowHeaderHook : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		
		protected string targetName = "";
		protected  TaskbarListener target;
		
		
		
		public string message = ""; // min,max,close
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
			if (ob.GetType() == typeof(TaskbarListener) )
			{
				target = ((TaskbarListener)ob);
				target.connected = connected = true;
				
				
				target.onChange += OnChange;
				
				OnChange();
				
			}
		}
		
		public void Send(string s) {
			target.message = message = s;
			target.OnChange();
		}
		public override void OnChange() {
			// behavior
			
		}
		

	}
}