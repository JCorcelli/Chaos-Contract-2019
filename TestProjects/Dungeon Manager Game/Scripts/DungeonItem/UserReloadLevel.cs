using UnityEngine;
using System.Collections;
using SelectionSystem;
using Utility.Tree;

namespace Dungeon
{
	
	public class UserReloadLevel : AbstractKeyHandler
	{
	
		
		
		protected override void OnRelease(){
			// may need to use a more complicated reset method, or checkpoint loading
			
			string level = DungeonVars.level;
			
			if (level == "Dungeon HUB")
				LoadingTree.Load(level, false);
			else
				// the dungeonvars level should determine what's loaded...
				LoadingTree.Load("Dungeon Init", false);
		}
		
		
	}

}