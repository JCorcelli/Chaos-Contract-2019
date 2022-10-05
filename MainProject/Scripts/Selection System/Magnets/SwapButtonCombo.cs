using UnityEngine;
using System.Collections;

using SelectionSystem.IHSCx;

namespace SelectionSystem.Magnets
{
	public class SwapButtonCombo : IHSCxConnectCombo
	{
		// I'm thinking this is more like a "SwapButtonCombo" type of deal. Calling it a dimension change is overboard.
		
		

		public GameObject deactivate;
		public GameObject activate;
		public GameObject hotspot;
		

		protected void Awake() {
			
			
			if (deactivate == null && activate == null) Debug.Log(name + " : please assign something to activate / deactivate.",gameObject);
		}
		protected override void OnPress(){
			// I could check if I'm blocked first
			
			// if not blocked
			
			if (hotspot != null)
				hotspot.SetActive(true); // alternatively change the mouse cursor and turn on a script that looks to see if I can drop.
			
		}
		
		protected override void OnRelease(){
			// I could check if I'm blocked first
			
			// if not blocked
			
			if (deactivate != null)
				deactivate.SetActive(false);

			if (activate != null)
				activate.SetActive(true);
		}
		

	
	}
}