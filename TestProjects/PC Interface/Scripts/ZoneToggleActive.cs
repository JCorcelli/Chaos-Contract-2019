using UnityEngine;
using System.Collections;

namespace Zone
{
	public class ZoneToggleActive : ZoneHubConnect {

		public GameObject target;
		
		
		protected override void OnEnable() {
			
			base.OnEnable();
			
			if (target == null) target = gameObject;

			OnChange();
		}
		protected override void OnChange() {
			if (hub.inZone) target.SetActive(true);
			else target.SetActive(false);
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			
		}
	}
}