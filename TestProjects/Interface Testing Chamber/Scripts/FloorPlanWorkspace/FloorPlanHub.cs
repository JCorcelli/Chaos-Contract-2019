using UnityEngine;
using System.Collections;

using System.Collections.Generic;


using SelectionSystem;

namespace FloorDesigner
{
	
	public class FloorPlanHub : ConnectHub {
		// The Dungeon2D FloorPlan Connect Hub
		
		// basic identifications
		
		public enum Channel {
			Default = 0
		}
		
		new public enum Enum {
			Ping = 0,
			ZoomBar = 1,
			ZoomRect = 2,
			ResizeHub = 3,
			DragHold = 4,
			Itom = 5
			
		}
		
		
	}
}