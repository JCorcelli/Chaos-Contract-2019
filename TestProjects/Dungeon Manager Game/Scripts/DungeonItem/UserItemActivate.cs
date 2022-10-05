using UnityEngine;
using System.Collections;
using SelectionSystem;


namespace Dungeon
{
	public class UserItemActivate : AbstractButtonHandler {

		public Transform target;
		
		public static UserItemActivate instance;
		
		protected void Awake() {
			if (instance != null)
			{
				Destroy(this);
				return;
			}
			instance = this;
		}
		protected override void OnRelease(){
			// Enter, hopefully

			DungeonItems.Spawn(target.position); 
			
		}
	}
}