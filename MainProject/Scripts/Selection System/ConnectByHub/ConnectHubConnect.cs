using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace SelectionSystem
{
	public class ConnectHubConnect : UpdateBehaviour {
		// This is intended for big applications that need to coordinate actions
		
		
		public ConnectHub hub;
		public ConnectHubDelegate onConnect;
		

		
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
			if (hub == null) hub = GetComponentInParent<ConnectHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			hub.onMessage -= OnMessage;
			hub.onConnect -= OnConnect;
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			
			//
			//Connect((int)Enumerator);
		}
		protected override void OnDisable( ){
			base.OnDisable();
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			
			hub.onConnect -= OnConnect;
			// something can check if this is disabled.
		}
		
		protected virtual void Connect() {
			
			hub.Connect( gameObject);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		
		
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			
			// every time something calls a message, in scope, this will check it
			
				
		}
		protected virtual void OnConnect(Object other) {
			// the use of this changes rapidly
		}
		
		

	}
}