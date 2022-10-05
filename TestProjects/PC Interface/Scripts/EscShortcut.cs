
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class EscShortcut : AbstractButtonHandler
	{
		// this is a leaf of the zone hub
		
		public bool escMenu = false;
		protected override void OnEnable(){
			base.OnEnable();
			escMenu = ZoneGlobal.escMenu;
		}
		protected override void OnPress(){
			if (ZoneGlobal.inZone) ZoneGlobal.inZone = false;
			else if (ZoneGlobal.escMenu) ZoneGlobal.escMenu = false;
			else
				ZoneGlobal.escMenu = true;
				
				
			escMenu = ZoneGlobal.escMenu;
			
			ZoneGlobal.OnChange();
		}
	}
}