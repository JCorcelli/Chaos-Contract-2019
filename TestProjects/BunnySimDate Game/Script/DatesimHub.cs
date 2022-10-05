using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SelectionSystem;

namespace Datesim
{
	
	public class DatesimHub : SelectionSystem.ConnectHub {
		
		public enum Channel {
			Default = 0,
			Option = 1,
			Effect = 2,
			BreakOut = 3
		}
		
		new public enum Enum
		{
			Ping =		0,
			Stage1 =	1,
			stage2 =	2,
			stage3 =	3,
			stage4 =	4,
			// 1 << 2
			EndDate =	5 // used by the end date stats
		}
		
		public enum OptionEnum {
			// to be altered to fit as needed
			None = 0, 
			ChangeTopic = 6
		}
		
		public enum BreakOutEnum {
			// later, this is 3D effect, as well as 2D
			LeaveZone = 0,
			ToggleOnBed = 1, 
			Charge = 2,
			Flinch = 3,
			PowerOff = 4 // pretty much ends the sim
		}
		
		public enum EffectEnum {
			DatesimNoAnimation = 0,
			DatesimHop = 1,
			DatesimToggleHide = 2, // ears are visible
			DatesimLaugh = 3,
			DatesimFlop = 4,
			DatesimThump = 5,
			DatesimKiss = 6,
			DatesimShake = 7 // triggers if you flinch, and you may leave, which quits the app
		}
	}
}