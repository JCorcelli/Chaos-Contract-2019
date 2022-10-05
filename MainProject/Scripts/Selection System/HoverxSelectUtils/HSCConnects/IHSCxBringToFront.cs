using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxBringToFront : IHSCxConnect {
		
		
		new protected Transform transform;
		protected override void OnEnable(){
			base.OnEnable();
			transform = GetComponent<Transform>();
			ih.onPress += OnPress;
		}
		protected override void OnDisable(){
			base.OnDisable();
			ih.onPress -= OnPress;
		}
		protected void OnPress( HSCxController caller ) {
				
			transform.SetSiblingIndex(transform.parent.childCount - 1);
			
		}
		
		 
	}
}