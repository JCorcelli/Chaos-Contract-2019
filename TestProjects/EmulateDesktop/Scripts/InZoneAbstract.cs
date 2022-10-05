
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class InZoneAbstract : UpdateBehaviour
	{
		// this is supposed to enable selection of things, or be a selectable
		
		
		public ZoneHub hub;
		public bool inZone = false;
		
		
		protected override void OnEnable() {
			
			base.OnEnable();
			
			//connect to zone hub
			
			if (hub == null) hub = GetComponentInParent<ZoneHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			hub.onMessage -= OnMessage;
			hub.onConnect -= OnConnect;
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			hub.onChange -= OnChange;
			hub.onChange += OnChange;
			OnChange();
		}
		
		
		protected virtual void OnMessage(int fromChannel, int msgSent) {}
		protected virtual void OnConnect(Object sender) {}
			
		
			
		protected virtual void OnChange() {
			
			inZone = ZoneGlobal.inZone;
			
		}
	}
}