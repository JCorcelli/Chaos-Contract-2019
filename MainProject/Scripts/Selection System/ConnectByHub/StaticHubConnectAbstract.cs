using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace SelectionSystem
{
	public abstract class StaticHubConnect : UpdateBehaviour {
		// This is a basic formulae of connecting with StaticHub
		
		
		// this line of code is for listeners
		public ConnectHubDelegate onChange;
		
		protected bool _connected = false;
		protected bool _subscribed = false;
		public bool connected{ get{return _connected;}set{ _connected = value;} }
		public bool subscribed{get{return _subscribed;}set{_subscribed = value;} }
		
		public virtual void CheckConnected(){
			if (!subscribed) SubscribeHub();
			/*
			if (connected) return;
			if (_hook) 
			{
				Ping();
			}
			else
			{
				Connect();
			}
			*/
		}
		public virtual void UnsubscribeHub(){
			subscribed = false;
			
			StaticHub.onConnect -= OnConnect;
			
			StaticHub.onPing -= OnPing;
		}
		public virtual void SubscribeHub(){
			subscribed = true;
			
			UnsubscribeHub();
			StaticHub.onConnect += OnConnect;
			
			StaticHub.onPing += OnPing;
		}
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			//CheckConnected();
			
		}
		protected void Destroy( ){
			
			// something can check if this is disabled.
			UnsubscribeHub();
		}
		
		protected virtual void Connect() {
			
			StaticHub.Connect(this);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		
		protected void Ping(){
			StaticHub.Ping();
		}
		protected virtual void OnPing() {
			Connect();
			
				
		}
		protected virtual void OnConnect(object ob) {
		}
		
		

		
		public virtual void OnChange(){
			
		}

	}
}