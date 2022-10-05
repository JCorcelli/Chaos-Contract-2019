using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace Dungeon.Save
{
	
	public class DungeonSaveRoom	: MonoBehaviour, IDungeonSaveStatic {
		// This goes in the spawned object
		
		public static string prefabPath = "DungeonBuildMode/RoomModule";
		
		public static string prefabName = "RoomModuleS";
		
		
		public static string savedName ="NoPreference";
		
		public void Save() {
			SpS saved = new SpS();
			
			saved.prefabPath = DungeonVars.resourceFolder+prefabPath; // necessary for direct object spawning
			
			saved.prefabName = prefabName; // I don't want it confused with some other prefab
			
			saved.savedName = savedName; // This should be unique for preference based items
			
			
			saved.Spawn(transform); // important, to set position
			saved.Save();
			
			// on transform.move, settransform, save
		}
	}
	
}