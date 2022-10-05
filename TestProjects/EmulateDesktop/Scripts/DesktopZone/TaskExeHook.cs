using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class TaskExeHook : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		
		protected string targetName = "";
		protected  TaskbarListener target;
		
		public bool running = false;
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
		
		public void StartExe() {
			running = target.running;
			if (running) return;
			target.StartExe();
		}
		
		public override void OnChange() {
			// behavior
			running = target.running;
		}
		

	}
}