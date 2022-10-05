using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Dungeon.Save
{
	public class UserSaveLevel : AbstractAnyHandler
	{

	
		protected override void OnEnable(){
			base.OnEnable();
			
		}
		protected override void OnRelease(){
			if (triggerName != "save") return;
			DungeonSave.instance.SaveLevel();
			
			
			
		}
	
	}
}