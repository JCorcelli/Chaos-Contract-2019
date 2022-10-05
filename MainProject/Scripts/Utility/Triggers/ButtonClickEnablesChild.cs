using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class ButtonClickEnablesChild : MonoBehaviour {

		public string targetName = "PresenceIndicator";
		
		private Transform child;
		
		public bool isActive = false;
		
		void Awake () { child = transform.GetChild(0);  }
		void Start () { child.gameObject.SetActive( isActive); }
		
		void OnMouseUpAsButton( ) {
			
			isActive = !isActive;
			child.gameObject.SetActive( isActive);
				
			
		}
	}
}