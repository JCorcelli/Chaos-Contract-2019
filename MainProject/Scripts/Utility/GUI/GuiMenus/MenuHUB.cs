using UnityEngine;
using System.Collections;

namespace Utility.GUI
{
	
	public interface IMenu
	{
		void IsOpen(bool set);
		bool IsOpen();
	}
	
	public class MenuHUB : MonoBehaviour {

		// Use this for initialization
		
		
		public IMenu currentMenu;
		
		protected void Start () {
			if (currentMenu != null)
				currentMenu.IsOpen(true);
		}
		
		public void ShowMenu(IMenu menu)
		{
			// close old menu
			if (currentMenu != null)
				currentMenu.IsOpen(false);
			
			
			// change current, open next menu
			currentMenu = menu;
			currentMenu.IsOpen(true);
		}
		
	}
}