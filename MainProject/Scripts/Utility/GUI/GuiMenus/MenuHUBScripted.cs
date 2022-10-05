using UnityEngine;
using System.Collections;

namespace Utility.GUI
{
	
	
	public class MenuHUBScripted : MonoBehaviour {

		// Use this for initialization
		
		
		public MenuScripted currentMenu;
		
		protected void Start () {
			if (currentMenu != null)
				currentMenu.IsOpen(true);
		}
		
		public void ShowMenu(MenuScripted menu)
		{
			// close old menu
			if (currentMenu != null)
				currentMenu.IsOpen(false);
			
			
			// change current, open next menu
			currentMenu = menu;
			currentMenu.IsOpen(true);
		}
		
		public void CloseMenu()
		{
			if (currentMenu == null) return;
			currentMenu.IsOpen(false);
			currentMenu = null;
		}
	}
}