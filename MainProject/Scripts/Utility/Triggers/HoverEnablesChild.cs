using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class HoverEnablesChild : MonoBehaviour {

		
		private Transform child;
		
		
		void Awake () { child = transform.GetChild(0);  }
		void Start () { child.gameObject.SetActive( false); }
		void OnMouseEnter( ) {
			child.gameObject.SetActive(true);
				
		}
		void OnMouseExit( ) {
				
			child.gameObject.SetActive(false);
				
			
		}
	}
}