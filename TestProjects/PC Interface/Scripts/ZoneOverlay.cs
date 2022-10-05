using UnityEngine;
using System.Collections;

namespace Zone
{
	public class ZoneOverlay : ZoneHubConnect {

		public string targetName = "PresenceIndicator";
		
		
		public CanvasGroup canvas;
		protected override void OnEnable() {
			
			base.OnEnable();
			if (canvas == null) canvas = GetComponent<CanvasGroup>();
			
			OnChange();
		}
		protected override void OnChange() {
			if (hub.inZone) canvas.alpha = 1f;
			else canvas.alpha = 0f;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			
		}
	}
}