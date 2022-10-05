
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class InFocusSignalPower	: InZoneSignal
	{
		// this is supposed to enable selection of things, or be a selectable
		
		
		public bool power_on = true;
		protected override void OnMessage(int channel, int msg){
			if (channel != 0) return;
			
			if (msg == 0) power_on = false;
			else if (msg == 1) power_on = true;
		}
		protected override void OnClick(){
			if (!hub.inFocus ) return;
			power_on = !power_on;
			if (power_on )
				hub.Send(0, 1);
			else
				hub.Send(0, 0);
		}
	}
}