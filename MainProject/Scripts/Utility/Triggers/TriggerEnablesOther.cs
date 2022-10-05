using UnityEngine;
using System.Collections;

namespace Utility.Triggers  
{
	public class TriggerEnablesOther : MonoBehaviour {

		public string targetName = "PresenceIndicator";
		
		[SerializeField] private Transform child;
		
		
		void OnEnable () { child.gameObject.SetActive( false); }
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