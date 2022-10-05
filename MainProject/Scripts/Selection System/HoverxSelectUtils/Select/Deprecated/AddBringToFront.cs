using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	public class AddBringToFront : SelectRelayAddAbstract {
		
		
		protected override void OnPress( ) {
			base.OnPress();
			
			transform.SetSiblingIndex(transform.parent.childCount - 1);
			
		}
		
		 
	}
}