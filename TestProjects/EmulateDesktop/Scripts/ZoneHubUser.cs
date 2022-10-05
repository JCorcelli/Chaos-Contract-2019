
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneHubUser : ZoneHubButtonHandler
	{
		// this is for laptop use, specifically, while standing near it
		
		public bool inFocus = false;
		public bool proximity = false;
		protected override void OnEnable(){
			base.OnEnable();
			
			
			OnChange();
		}
		protected override void OnChange(){
			inZone = ZoneGlobal.inZone;
			proximity = hub.proximity;
			if (inFocus != hub.inFocus) 
				inFocus = !inFocus;
			else if (inFocus && !hub.proximity)
				CancelFocus();
				
			
		}
			
			
		protected override void OnPress(){
			if (!hub.proximity) return;
			if (triggerName == "cancel" || triggerName == "inventory") 
			{
				CancelFocus();
				return;
			}
			
			if (inFocus ) return;
			if (triggerName == "use") 
			{
				inFocus = hub.inFocus = true;
				hub.OnChange();
			}
		}
		protected void CancelFocus(){
			
			if (!inFocus ) return;
			inFocus = hub.inFocus = false;
			
			hub.OnChange();
		}
	}
}