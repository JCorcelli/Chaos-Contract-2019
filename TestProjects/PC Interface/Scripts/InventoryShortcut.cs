
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class InventoryShortcut : AbstractButtonHandler
	{
		// this is a leaf of the zone hub
		
		public bool inInv = false;
		protected override void OnEnable(){
			base.OnEnable();
			inInv = ZoneGlobal.inInv;
		}
		protected override void OnPress(){
			if (ZoneGlobal.escMenu) return;
			ZoneGlobal.inInv = !ZoneGlobal.inInv;
			inInv = ZoneGlobal.inInv;
			ZoneGlobal.OnChange();
		}
	}
}