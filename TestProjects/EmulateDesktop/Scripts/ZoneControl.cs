
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneControl : AbstractButtonHandler
	{
		// this is a leaf of the zone hub
		
		public bool inZone = false;
		protected override void OnEnable(){
			base.OnEnable();
			inZone = ZoneGlobal.inZone;
		}
		protected override void OnPress(){
			ZoneGlobal.inZone = !ZoneGlobal.inZone;
			inZone = ZoneGlobal.inZone;
			ZoneGlobal.OnChange();
		}
	}
}