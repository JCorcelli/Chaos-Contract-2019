using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility
{
	public class TriggerNode : MonoBehaviour {

		
		public TriggerHUB manager;
		
		protected virtual void Awake () {
			
			if (manager == null)
			{
				Debug.Log(gameObject + " Warn: Requires a TriggerHUB to function",gameObject);
			}
			
		}
		
		protected void OnEnable() {
			manager.onTriggerEnter += onTriggerEnter;
			manager.onTriggerExit += onTriggerExit;
		}
		protected void OnDisable() {
			manager.onTriggerEnter -= onTriggerEnter;
			manager.onTriggerExit -= onTriggerExit;
		}
		
		protected virtual void onTriggerEnter () {
		
		}
		
		
		protected virtual void onTriggerExit () {
		
		}
	}
}