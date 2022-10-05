using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utility.Tree;
using SelectionSystem;

namespace Dungeon
{
	
	public class UserCycleLevelSelect : AbstractButtonHandler {
		
		// specific inventory commands
		// could also be user determined
		protected string invPrev = "inv previous"; // q
		protected string invNext = "inv next"; // e
		
		
		public static UserCycleLevelSelect instance;
		public static List<string> dLevels = new List<string>(){"Dungeon HUB","Dungeon 1","Dungeon 2","Dungeon 3","Dungeon 4"};
		public static int dLevelIndex = 0;
		
		
		protected virtual void Start() {
			if (instance != null) 
			{
				Destroy(this);
				return;
			}
			instance = this;
			
			
		}
		protected override void _OnPress(){
			// update allows multiple buttons
			OnPress();
		}
		
		protected override void OnPress() {
			// if (Input.GetButtonDown( buttonName) ){}
		
			if (Input.GetButtonDown( invPrev) ){
				pressed = true;
				OnPrev();
			}
			if (Input.GetButtonDown( invNext) ){
				pressed = true;
				OnNext();
			}
				
		}
		
		protected void OnPrev() {
			dLevelIndex --;
			if (dLevelIndex < 0)
				dLevelIndex = dLevels.Count - 1;
		}
		protected void OnNext() {
			dLevelIndex ++;
			if (dLevelIndex > dLevels.Count - 1)
				dLevelIndex = 0;
		}
		
	}

}