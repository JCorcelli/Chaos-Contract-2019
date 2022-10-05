using UnityEngine;
using System.Collections;

namespace Zone
{
	public class ZoneHubProximity : ZoneHubConnect {

		public string targetName = "PresenceIndicator";
		
		new public Collider collider;
		protected override void OnEnable() {
			
			base.OnEnable();
			
			collider = GetComponent<Collider>();
			OnChange();
		}
		protected override void OnChange() {
			
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			
		}
		protected void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				
				hub.proximity = true;
				hub.OnChange();
			}
		}
		
		protected void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				hub.proximity = false;
				hub.OnChange();
				
			}
			
		}
	}
}