using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utility.Tree;
using SelectionSystem;

namespace Dungeon
{
	
	public class UserSceneChange : AbstractButtonHandler {
		
		public string level = "";
		public string exit = "";
	
		
		
		protected static UserSceneChange instance;
		
		protected void Awake() {
			
			if (instance != null) 
			{
				Destroy(this);
				return;
			}
			instance = this;
		
		}
		protected void CallLoad() {
			
			DungeonVars.exit = exit;
			
			level = UserCycleLevelSelect.dLevels[UserCycleLevelSelect.dLevelIndex];
			
			DungeonVars.level = level;
			
			LoadingTree.Load(level, false);
			
		}
		
		
		protected override void OnPress( ) {
			
			CallLoad();
				
		}
		
		protected override void OnUpdate( ) {
			base.OnUpdate();
			level = UserCycleLevelSelect.dLevels[UserCycleLevelSelect.dLevelIndex];
				
		}
		
		
	}

}