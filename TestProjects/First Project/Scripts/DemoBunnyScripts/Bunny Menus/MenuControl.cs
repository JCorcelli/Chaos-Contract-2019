using UnityEngine;
using System.Collections;

namespace PlayerAssets.Interface
{
	public class MenuControl : MonoBehaviour {

		// Use this for initialization
		
		private GameObject topMenu;
		private bool guiOn = false;
		void Awake () {
			topMenu = this.transform.GetChild(0).gameObject;
			topMenu.SetActive( false );
		}
		
		// Update is called once per frame
		
		public void ShowGui(bool f) {
			guiOn = f;
			topMenu.SetActive( f );
			
			
		}
		
		
		
		void Update () {
			
			if (!guiOn && PlayerInterface.locked)
			{
				
				return;
			}
			
			if (PlayerInterface.looking) return;
			if (Input.GetKeyDown("space")) // playerinterface.menu
			{
				guiOn = !guiOn;
				
				topMenu.SetActive (guiOn) ;
				
			}
		}
	}
}