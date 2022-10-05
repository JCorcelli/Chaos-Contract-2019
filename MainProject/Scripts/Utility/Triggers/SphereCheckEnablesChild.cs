using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class SphereCheckEnablesChild : MonoBehaviour {

		public string targetName = "PresenceIndicator";
		
		private Transform child;
		
		
		void Awake () { child = transform.GetChild(0);  }
		void Start () { child.gameObject.SetActive( false); }
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				
				child.gameObject.SetActive(true);
				
			}
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				child.gameObject.SetActive(false);
				
			}
			
		}
	}
}