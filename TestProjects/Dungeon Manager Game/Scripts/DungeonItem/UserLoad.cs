using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Dungeon.Save
{
	public class UserLoad : AbstractAnyHandler
	{

	
		protected override void OnEnable(){
			base.OnEnable();
			
		}
		protected override void OnRelease(){
			if (triggerName != "reload") return;
			DungeonSave.instance.LoadLevel();
			
			Debug.Log("Reload level!");
			// need to force the level to reload when I do a loading sequence though
			
			
		}
	
	}
}