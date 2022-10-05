using UnityEngine;
using System.Collections;

namespace Datesim
{
	public class DatesimProximity : DatesimAppConnectLite {

		public string targetName = "PresenceIndicator";
		
		new public Collider collider;
		protected override void OnEnable() {
			
			base.OnEnable();
			
			collider = GetComponent<Collider>();
			OnChange();
		}
		protected override void OnChange() {
			collider.enabled = vars.power_on;
			
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			collider.enabled = false;
			
		}
		protected void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				
				vars.proximity = true;
				vars.OnChange();
			}
		}
		
		protected void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				vars.proximity = false;
				vars.OnChange();
				
			}
			
		}
	}
}