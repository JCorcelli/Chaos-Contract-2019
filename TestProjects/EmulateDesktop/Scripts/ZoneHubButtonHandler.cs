
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneHubButtonHandler : AbstractAnyHandler
	{
		// this is for laptop use, specifically, while standing near it
		public ZoneHub hub;
		public bool inZone = false;
		
		protected override void OnEnable(){
			base.OnEnable();
			//connect to zone hub
			
			if (hub == null) hub = GetComponentInParent<ZoneHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			
			hub.onChange += OnChange;
		}
		
		
		protected virtual void OnChange() {
			inZone = ZoneGlobal.inZone;
		}
		protected virtual void OnMessage(int fromChannel, int msgSent) {}
		protected virtual void OnConnect(Object sender) {
			
		}
		
		protected override void OnPress(){
			if (inZone || !hub.proximity) return;
			ZoneGlobal.inZone = true;
			inZone = true;
			ZoneGlobal.OnChange();
		}
	}
}