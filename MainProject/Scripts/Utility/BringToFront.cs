using UnityEngine;
using System.Collections;


namespace Utility
{
	
	
	public class BringToFront : MonoBehaviour {
		
		
		protected void Awake( ) {
			
			transform.SetSiblingIndex(transform.parent.childCount - 1);
			
		}
		
		 
	}
}