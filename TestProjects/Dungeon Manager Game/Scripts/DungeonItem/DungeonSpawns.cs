using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dungeon
{
	public class DungeonSpawns : MonoBehaviour {
		// A test spawner
	
		public static string spawnedLevel = "XXDDeeEE";
		
		public Transform _theItem;
		public Transform spawnedItemGroup;
		
		protected static Vector3 spawnPos;
		// remember if you set variables to make the list static
		
		
		public static DungeonSpawns instance;
		
		public static bool spawned = false;
		protected void Awake () {
			
			instance = this;
			
			if (spawnedItemGroup == null) 
				spawnedItemGroup = gameObject.transform;
			
			Load();
		}
		protected void Load () {
			if (_theItem == null) 
			{
				Debug.LogError("your debugging of theItem is failing", gameObject);
				return;
			}
			
			if (spawnedItemGroup == null)
			{
				Debug.LogError("your debugging of spawnedItemGroup is failing", gameObject);
				return;
			}
				
			// Debug.Log("spawn: "+spawnedLevel+" if "+DungeonVars.level);
			if (spawned && DungeonVars.level == spawnedLevel)
				_Spawn();
				
		}
		
		protected void _Spawn() {
			
			Transform t = Instantiate(_theItem); // and now I just need a way to change the dungeon name !!!!
			
			t.parent = spawnedItemGroup;
			t.position = spawnPos;
			
		}
		
		
		public static void Spawn(Vector3 pos) {
			spawned = true;
			spawnPos = pos;
			instance._Spawn();
		}
		
	}
}