
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneShortcut : AbstractButtonHandler
	{
		// this is a leaf of the zone hub
		
		public bool inZone = false;
		protected override void OnEnable(){
			base.OnEnable();
			inZone = ZoneGlobal.inZone;
		}
		protected override void OnPress(){
			if (ZoneGlobal.escMenu) return;
			ZoneGlobal.inZone = !ZoneGlobal.inZone;
			inZone = ZoneGlobal.inZone;
			ZoneGlobal.OnChange();
		}
	}
}