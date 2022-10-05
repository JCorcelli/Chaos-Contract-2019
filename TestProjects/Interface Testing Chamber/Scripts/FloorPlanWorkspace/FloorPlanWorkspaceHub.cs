using UnityEngine;
using System.Collections;

using SelectionSystem;

namespace FloorDesigner
{
	
	public class FloorPlanWorkspaceHub : ConnectHub {
		
		
		public enum Channel {
			Default = 0
			
		}
		
		new public enum Enum
		{
			Ping =		0,
			// ID codes
			Rect =		1, // drag & resize
			Snap = 		2, // room wall
			Draw = 		3, // build
			Select =	4, // basic, drag
			Copy = 		5, // shortcut
			Extension =	6, // S,LO
		
			
			// SnapEnum 
			
			SnapWalls = 	21,
			SnapRooms = 	22,
			
			// Drawenum
			DrawFree = 		31,
			DrawLine = 		32,
			
			// SelectEnum
			SelectTool = 		41,
			SelectIncludeRooms = 	42,
			SelectIncludeItoms = 	43,
		
			// CopyEnum
			CopyMove		= 51,
			CopyTool		= 52,
			CopyPaste		= 53,
			
			// ExtensionEnum
			ExtensionStart = 61, // hot swap start
			ExtensionEnd = 	62, // hot swap end
			ExtensionSave = 	63,
			ExtensionLoad = 	64
			// This includes an Override system.  Meant for additional tools.
			
			// Set 1 is saved on quit. Save/Load regards set 1. 
			// Saving workspaces should be very similar.
			
			// Set 2 begins/ends when respective enum is called. Forgotten on quit.
			
			
		}
		
		
		
	}
}