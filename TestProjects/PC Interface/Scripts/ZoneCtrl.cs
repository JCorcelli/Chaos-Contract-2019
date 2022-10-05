
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneCtrl : AbstractButtonHandler
	{
		// this is a leaf of the zone hub
		
		public bool inZone = false;
		protected override void OnEnable(){
			base.OnEnable();
			inZone = ZoneGlobal.inZone;
		}
		protected override void OnPress(){
			if (ZoneGlobal.escMenu) return;
			ZoneGlobal.inZone = inZone = true;
			ZoneGlobal.OnChange();
		}
		protected override void OnRelease(){
			ZoneGlobal.inZone = inZone = false;
			ZoneGlobal.OnChange();
		}
	}
}