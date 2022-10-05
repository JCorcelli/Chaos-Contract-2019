
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public class StaticMediaHubHook  {
		// This is a serious go-between
		public StaticMediaHub hub;
		public ConnectHubDelegate onChange;
		
		public bool connected = false;
		public bool subscribed = false;
		public virtual void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			if (connected && hub != null) return;
			// hook
			Ping();
			
			//Connect();
			
		}
		protected void Ping(){
			StaticHub.Ping();
		}
		
		public virtual void UnsubscribeHub(){
			subscribed = false;
			
			StaticHub.onConnect -= OnConnect;
			
		}
		
		public virtual void SubscribeHub(){
			subscribed = true;
			
			UnsubscribeHub();
			StaticHub.onConnect += OnConnect;
			
		}
		
		
		protected virtual void OnConnect(object ob) {
			// for hooks
			
			if (hub == null && ob.GetType() == typeof(StaticMediaHub) )
			{
				hub = ((StaticMediaHub)ob);
				
				connected = true;
				
				hub.onChange -= OnChange;
				hub.onChange += OnChange;
				
				
			}
		}
		
		
		public StaticMediaHubHook( ){
			
			
			CheckConnected();
			
			
			
		}
		
		protected virtual void OnChange() {if (onChange != null) onChange();}
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			
		}
		
		
		protected virtual void OnDestroy( ){
			if (hub == null) return;
			
			hub.onChange -= OnChange;
		}

	}
}