using UnityEngine;
using System.Collections;

namespace Utility.Managers
{
	
	public class TriggerHUB : AbstractTriggerHUB {

		public string targetName = "PresenceIndicator";
		
		public int count = 0;
		
		void OnDisable() {
			
			if (count > 0)
			{
				Debug.Log("Disabling hub with " + count + " touching objects.", gameObject);
			
				count = 0;
				if (onTriggerExit != null) onTriggerExit();
			}
		}
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				if (count < 1)
				{
					if (count < 0) count = 0;
					if (onTriggerEnter != null) onTriggerEnter();
				}
				
				count ++;
				
				
			}
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count < 0) count = 0;
				if (count == 0)
				{
					if (onTriggerExit != null) onTriggerExit();
				}
				
			}
			
		}
		
		
	}
}