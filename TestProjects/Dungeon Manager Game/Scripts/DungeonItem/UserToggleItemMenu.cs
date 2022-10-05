using UnityEngine;
using System.Collections;
using SelectionSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Utility.GUI;

namespace Dungeon
{
	public class UserToggleItemMenu : AbstractButtonComboPrecision {

		public MenuScripted target;
		public MenuHUBScripted hub;
		
		
		protected void Awake() {
			hub = GetComponentInParent<MenuHUBScripted>();
			
		}
		protected override void OnRelease(){
			// Enter, hopefully
			
			GameObject g = EventSystem.current.currentSelectedGameObject;
			if (g != null && g.GetComponent<InputField>() != null) return;

			if (target.IsOpen())
				hub.CloseMenu();
			else
				hub.ShowMenu(target); // will take care of any issues with it being opened? hopefully.
			
		}
	}
}