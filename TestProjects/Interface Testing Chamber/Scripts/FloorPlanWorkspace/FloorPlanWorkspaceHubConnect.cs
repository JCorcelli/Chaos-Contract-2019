using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanWorkspaceHubConnect : UpdateBehaviour {
		// This is intended to connect, only to delegate actions from the hub
		
		
		public FloorPlanWorkspaceHub hub;
		public ConnectHubDelegate onConnect;

		public FloorPlanWorkspaceHub.Channel channel = FloorPlanWorkspaceHub.Channel.Default;
		
		public bool connected = true;
		protected virtual IEnumerator RepeatConnect() {
			
			while (!connected) 
			{
				yield return new WaitForSeconds(.2f);
				
				hub.Ping();
			}
			yield return null;
		}
		
		protected void StartReconnect() {
			
			connected = false;
			StartCoroutine("RepeatConnect");
		}
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (hub == null) hub = GetComponentInParent<FloorPlanWorkspaceHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			hub.onMessage -= OnMessage;
			hub.onConnect -= OnConnect;
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			
			Connect();
		}
		
		protected virtual void Connect() {
			
			hub.Connect( gameObject);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		
		protected override void OnDisable( ){
			base.OnDisable();
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			
			hub.onConnect -= OnConnect;
			// something can check if this is disabled.
		}
		
		
		
		
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			
			if (msgEnum == 0) Connect() ;
			// every time something calls a message, in scope, this will check it
			
				
		}
		protected virtual void OnConnect(Object other) {
			// there's no telling which will connect first. Only one needs to use the connection, whether it's here or OnEnable.
			
			// if (thisenum == (int)Enumerator) 
			// connected = other;
			// hub.connected = gameObject;
			
			// use the other gameObject
			// if onconnect != null onConnect() something else grabs the connection, done
		}
		
		

	}
}