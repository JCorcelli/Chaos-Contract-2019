using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Animations;

namespace Dungeon
{
	public class UseDoorByHandlexKey : UseDoor {

		// This assumes an entity with given name will touch it
		
		// action 1: walk over grab door (nobody else can use it), immediately open door
		// action 2: walk away let go of door (door closes)
		
		
		public string targetName = "PlayerCollider";
		public bool autoclose = false;
		///public bool autoOpen = false;
		public string lockName = "";
		
		public int count = 0; 
		

		void OnTriggerEnter( Collider col ) {
			
			if (targetName == col.name) {
				count ++;
				KeyOpen();
				
			}
		}
		
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					count = 0;
					if (!autoclose) return;
					Close();
				}
				
			}
			
		}
		
		protected void KeyOpen() {
			
			if (lockName != "" && !DungeonVars.keyNames.Contains(lockName)) return;
			Open();
		}
		///protected override void OnUpdate() {
		///	if (autoOpen) KeyOpen();
		///	
		///}
		
		
	}
}
