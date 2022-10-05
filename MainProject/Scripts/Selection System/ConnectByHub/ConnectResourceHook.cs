
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SelectionSystem
{
	public class ConnectResourceHook  {
		// This is intended to be local resources of a 'super' gameobject
		
		
		public ConnectResource _hub;
		public ConnectResource hub{
			get{ return _hub;}
			set{
			_hub.onMessage -= OnMessage;
			_hub.onConnect -= OnConnect;
			
			_hub = value;
			if (_hub == null) return;
			_hub.onMessage += OnMessage;
			_hub.onConnect += OnConnect;
			}
		}
		public ConnectHubDelegate onConnect;
		

		
		public bool connected = true;
		
		
		protected void OnDestroy( ){
			
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			
			hub.onConnect -= OnConnect;
			// something can check if this is disabled.
		}
		
		public virtual void Connect() {
			
			hub.Connect( this);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		
		
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			if (msgEnum == 0) Connect();
			// every time something calls a message, in scope, this will check it
			
				
		}
		protected virtual void OnConnect(object other) {
			// the use of this changes rapidly
		}
		
		

	}
}