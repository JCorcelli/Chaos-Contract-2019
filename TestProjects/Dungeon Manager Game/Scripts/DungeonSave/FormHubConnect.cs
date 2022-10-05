using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

using Dungeon;

namespace Dungeon.Save
{
	public class FormHubConnect : UpdateBehaviour {
		
		
		
		public FormHub hub;
		public FormHubDelegate onConnect;
		protected GameObject connected;

		protected override void OnEnable( ){
			base.OnEnable();
			if (hub == null) hub = GetComponentInParent<FormHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			hub.onMessage -= OnMessage;
			hub.onConnect -= OnConnect;
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			
			//
			//GameObject g = Connect((int)Enumerator);
		}
		
		protected override void OnDisable( ){
			base.OnDisable();
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			
		}
		
		
		protected virtual bool Connect(int target) {
			// retarget
			
			
			hub.Connect(target, gameObject);
			
			if (hub.connected == null) {
				Debug.Log("no connection, this broke", gameObject);
				return false; 
			}
			
			connected = hub.connected;
			hub.Disconnect();
			return true;
		}
		
		protected virtual void OnMessage(int thisenum, string s) {
			
			// if (thisenum == (int)Enumerator) check string, do a thing
				
		}
		protected virtual void OnConnect(int thisenum, GameObject other) {
			// there's no telling which will connect first. Only one needs to use the connection, whether it's here or OnEnable.
			
			// if (thisenum == (int)Enumerator) 
			// connected = other;
			// hub.connected = gameObject;
			
			// use the other gameObject
			// if onconnect != null onConnect() something else grabs the connection, done
		}
		
		

	}
}