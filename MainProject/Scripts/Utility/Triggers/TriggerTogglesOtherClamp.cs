using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utility.Triggers  
{
	public class TriggerTogglesOtherClamp : MonoBehaviour {

		protected int count = 0; 
		public string targetName = "PresenceIndicator";
		
		public List<GameObject> activatedThings =  new List<GameObject>(); 
		public List<GameObject> deactivatedThings = new List<GameObject>(); 
		
		
		void Awake () { 
			foreach (GameObject g in activatedThings)
			{
				g.SetActive(false);
			}
		}
		public bool oneshot = true;
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
				foreach (GameObject g in activatedThings)
				{
					g.SetActive(true);
				}
				foreach (GameObject g in deactivatedThings)
				{
					g.SetActive(false);
				}
				
				if (oneshot)
					Destroy(this);
			}
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					foreach (GameObject g in activatedThings)
					{
						g.SetActive(false);
					}
					foreach (GameObject g in deactivatedThings)
					{
						g.SetActive(true);
					}
				}
				
			}
			
		}
	}
}