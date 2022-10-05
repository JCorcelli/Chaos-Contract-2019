using UnityEngine;
using System.Collections;
using Animations;

namespace Dungeon
{
	public class AddKeyToHandlexKey : UseItem {

		public string targetName = "PlayerCollider";
		public string lockName = "";
		public int count = 0; 
		protected void OnTriggerEnter( Collider col ) {
			if (count > 0) return;
			
			if ( targetName == col.name ) {
				if (DungeonVars.keyNames.Contains(lockName)) return;
				count ++;
				
				if (IsTaken()) return;
				AddKey();
				Grab();
				
				
			}
		}
		
		protected void AddKey( ) {
			DungeonVars.keyNames.Add(lockName);
		}
		
	}
}