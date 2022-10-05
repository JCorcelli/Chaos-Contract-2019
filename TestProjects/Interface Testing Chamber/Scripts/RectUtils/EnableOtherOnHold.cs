using System;
using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
	
	
    public class EnableOtherOnHold : AbstractButtonHandler
    {
		
		
		
		public GameObject other;
		
		
		protected UISelectMask sMask;
		
		protected override void OnEnable() {
			base.OnEnable();
			sMask = GetComponentInParent<UISelectMask>();
			
			if (sMask == null) Debug.Log("this requires a selection mask component in parent", gameObject);
			
			if (other == null) Debug.Log("set other to a gameObject", gameObject);
			other.SetActive(false);
		}
		
		
		
		protected override void OnRelease() {
			pressed = false;
			other.SetActive(false);
		}
		protected override void OnPress(){
			if (sMask.isHovered) 
			{
				pressed = true;
			}
			else
			{
				pressed = false;
				return;
			}
			
			
			
			other.SetActive(true);
			
			
		}
		
		 
		 
    }
}
