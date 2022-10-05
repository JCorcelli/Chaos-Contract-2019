using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class TriggerEnablesChildClamp : MonoBehaviour {

		public string targetName = "PresenceIndicator";
		
		private Transform child;
		
		/*
		if it's rigidbody it can have more than one collider triggered at once 
		// if it's any it can have more than one collider enter it at once, I need to keep count because of these pitfalls
		*/
		void Awake () { 
			child = transform.GetChild(0);  
			child.gameObject.SetActive( false);
		}
		
		public bool oneshot = true;
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				child.gameObject.SetActive(true);
				if (oneshot)
					Destroy(this);
			}
		}
		
	}
}