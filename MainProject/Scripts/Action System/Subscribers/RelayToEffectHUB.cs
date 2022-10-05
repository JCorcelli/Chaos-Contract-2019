using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ActionSystem.OnActionScripts;
using Utility.Managers;

namespace ActionSystem.Subscribers
{
	
	public class RelayToEffectHUB : WhoSubscriber {
		// soft subscriber is OnAction, after any alterations
		
		protected EffectHUB effect;
		
		protected void Awake() { 
			effect = GetComponent<EffectHUB>();
			if (effect == null) {
				Debug.Log(name + " issue: EffectHUB needed",gameObject);
				enabled = false;
			}
			
		}
		
		protected override void OnAction(ActionEventDetail data) {
			effect.Submit(data);
		}
	}
}