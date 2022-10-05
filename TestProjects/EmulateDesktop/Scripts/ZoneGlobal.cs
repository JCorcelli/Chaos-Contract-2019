
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneGlobal
	{ 
		// ui state is a sentinel layer above zone. Because there is pretty good chance a cheap message system is useful.
		
		// inZone "connects" the more common zone interface
		
		// outside zone, this shouldn't matter
		
		
		public static bool inZone = false;
		public static bool escMenu = false;
		public static bool inInv = false;
		
		public static ConnectHubDelegate onChange;
		public static void OnChange(){
			if (onChange != null) onChange();
		}
		
	}
}