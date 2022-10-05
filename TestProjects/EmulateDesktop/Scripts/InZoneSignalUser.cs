
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class InZoneSignalUser	: InZoneSignal
	{
		// this is supposed to enable selection of things, or be a selectable
		
		
		public bool inFocus = false;
		public bool proximity = false;
		protected override void OnEnable(){
			base.OnEnable();
			OnChange();
		}
		
		protected override void OnChange(){
			inZone = ZoneGlobal.inZone;
			proximity = hub.proximity;
			inFocus = hub.inFocus;
		}
		
		protected override void OnClick(){
			if (inFocus || !inZone || !hub.proximity) return;
			
			inFocus = hub.inFocus = true;
			hub.OnChange();
		}
	}
}