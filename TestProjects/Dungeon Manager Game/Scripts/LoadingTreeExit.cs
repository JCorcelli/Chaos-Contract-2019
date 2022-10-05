using UnityEngine;
using System.Collections;
using Utility.Tree;

namespace Dungeon
{
	
	public class LoadingTreeExit : MonoBehaviour {
		public string level = "";
		public string exit = "";
	
		
		
		
		protected void Awake() {
			
			if (level == "") Debug.LogError("Level not entered.");
		
		}
		protected void CallLoad() {
			
			DungeonVars.exit = exit;
			DungeonVars.level = level;
			
			//hack
			if (level == "Dungeon HUB")
				LoadingTree.Load(level, false);
			else
				// the dungeonvars level should determine what's loaded...
				LoadingTree.Load("Dungeon Init", false);
			
		}

		public string targetName = "PlayerCollider";
		
		
		public int count = 0; 

		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
					CallLoad();
				
			}
		}
		
		
	}

}