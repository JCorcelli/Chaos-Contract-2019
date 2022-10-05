using UnityEngine;
using System.Collections;

namespace Zone
{
	public class InventoryOverlay : ZoneHubConnect {

		public string targetName = "PresenceIndicator";
		
		
		public CanvasGroup canvas;
		protected override void OnEnable() {
			
			base.OnEnable();
			if (canvas == null) canvas = GetComponent<CanvasGroup>();
			
			OnChange();
		}
		protected override void OnChange() {
			if (hub.inInv) 
			{
				canvas.alpha = 1f;
			
				canvas.blocksRaycasts = true;
			}
			else 
			{
				canvas.alpha = 0f;
				canvas.blocksRaycasts = false;
			}
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			
		}
	}
}