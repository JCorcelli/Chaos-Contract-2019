using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	public class OnPressHoldSetActive: OnPressHold {
		
		
		public GameObject target;
		public bool active = false;
		protected override void OnCall(){
			target.SetActive(active);
		}
		 
	}
}