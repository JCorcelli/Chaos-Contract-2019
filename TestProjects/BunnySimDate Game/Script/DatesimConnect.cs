using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimConnect : SelectAbstract {
		// This is intended to connect, only to delegate actions from the hub
		
		
		public DatesimHub hub;
		public ConnectHubDelegate onConnect;

		public DatesimHub.Channel channel = DatesimHub.Channel.Default;
		
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
			if (hub == null) 
			{
				hub = GetComponentInParent<DatesimHub>();
				
			}
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return;
			}
			hub.onConnect -= OnConnect;
			hub.onConnect += OnConnect;
			
			
		}
		
		protected virtual void Connect() {
			if (hub == null) return;
			hub.Connect( this);
			
			
		}
		
		
		protected virtual void Destroy( ){
			
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			hub.onConnect -= OnConnect;
		}
		
		
		
		
		protected virtual void OnMessage(int channel, int rMessage) {}
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