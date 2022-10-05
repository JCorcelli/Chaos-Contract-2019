using UnityEngine;
using System.Collections;
using SelectionSystem;

using Utility.GUI;

namespace Dungeon
{
	public class UserCancelMenu : AbstractButtonHandler {

		public MenuHUBScripted hub;
		
		
		protected void Awake() {
			hub = GetComponentInParent<MenuHUBScripted>();
			
		}
		protected override void OnRelease(){
			// Enter, hopefully

			hub.CloseMenu(); // will take care of any issues with it being opened? hopefully.
			
		}
	}
}