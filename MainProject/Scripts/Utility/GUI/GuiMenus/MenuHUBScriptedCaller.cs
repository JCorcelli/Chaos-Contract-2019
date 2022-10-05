using UnityEngine;
using System.Collections;

using SelectionSystem;


namespace Utility.GUI
{
	
	public class MenuHUBScriptedCaller : SelectAbstract {

		// Use this for initialization
		
		public enum MenuEnum {
			ShowMenu = 1,
			CloseMenu = 2
		}
		
		protected MenuHUBScripted hub;
		public MenuScripted targetMenu;
		public MenuEnum menuAction;
		
		protected virtual void Awake () {
			
			hub = GetComponentInParent<MenuHUBScripted>();
			
			if (targetMenu == null) targetMenu = GetComponentInParent<MenuScripted>();
		}
		
		protected override void OnEnable() {
			base.OnEnable();
			
		
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			
		}
		
		protected override void OnClick()
		{
			if (menuAction == MenuEnum.ShowMenu)
				ShowMenu();
			else if (menuAction == MenuEnum.CloseMenu)
				CloseMenu();
			
		}
		
		
		protected void ShowMenu()
		{
			hub.ShowMenu(targetMenu);
		}
		
		protected void CloseMenu()
		{
			hub.CloseMenu();
		}
	}
}