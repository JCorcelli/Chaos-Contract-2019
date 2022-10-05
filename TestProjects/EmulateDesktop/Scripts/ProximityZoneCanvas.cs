
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ProximityZoneCanvas : InZoneAbstract
	{
		// this is supposed to enable selection of things, or be a selectable
		
		public Canvas canvas;
		
		public bool available = true;
		
		
		protected void CheckAvailable(){
			
			available = inZone && hub.proximity;
		}
		
		
		protected override void OnEnable() {
			
			base.OnEnable();
			OnChange();
			
		}
		
		
		
		protected override void OnDisable(){
			base.OnDisable();
			
		}
		
		
		
			
		protected override void OnChange() {
			
			base.OnChange();
			if (canvas == null) canvas = gameObject.GetComponent<Canvas>();
			CheckAvailable();
			canvas.enabled = available;
		}
		
		
	}
}