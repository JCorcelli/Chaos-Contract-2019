
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class InZoneSignal	: SelectAbstract
	{
		// while in zone you can click things, but what does it do?
		
		
		public ZoneHub hub;
		public bool inZone = false;
		
		new public Collider collider;
		protected override void OnEnable(){
			base.OnEnable();
			
			
			collider = GetComponent<Collider>();
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
		
		protected virtual void OnChange() {
			
			inZone = ZoneGlobal.inZone;
			
			// if (!inZone) DEACTIVATE STUFF NOW
			// 
		}
		protected virtual void OnMessage(int fromChannel, int msgSent) {}
		protected virtual void OnConnect(Object sender) {
			
		}
		protected override void OnClick(){
			if (!inZone) return;
		}
	}
}