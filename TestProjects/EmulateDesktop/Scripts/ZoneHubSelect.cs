
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneHubSelect : SelectAbstract
	{
		// this is a leaf of the zone hub
		
		public ZoneHub hub;
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
			
			hub.onChange -= OnChange;
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			
			hub.onChange += OnChange;
		}
		
		
		protected virtual void OnChange() {}
		protected virtual void OnMessage(int fromChannel, int msgSent) {}
		protected virtual void OnConnect(Object sender) {
			
		}
	}
}