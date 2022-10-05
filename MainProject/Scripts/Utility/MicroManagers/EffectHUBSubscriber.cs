using UnityEngine;
using System.Collections;

using ActionSystem.Subscribers;
using ActionSystem;

namespace Utility.Managers
{
	public abstract class EffectHUBSubscriber : AbstractSubscriber {
		/*
			does not globalize
			
			
		*/
		
		
		public EffectHUB hub;
		protected virtual void Awake () {
			// during initialization and whenever it is enabled later, this will subscribe/resubscribe to either manager or current ActionHandler
			
			
			// If I am hooking into ActionManager
			if (hub == null)
				hub = gameObject.GetComponentInParent<EffectHUB>();
			if (hub == null)
				Debug.Log(gameObject.name + ": Warn: no HUB manager or Group HUB",gameObject);
			
				
		}	
		protected override void OnEnable () {
			// during initialization and whenever it is enabled later, this will subscribe/resubscribe to manager
			
			
			// If I am hooking into HUB
			if (hub != null)
				hub.onAction += _OnAction;
			
				
				
		}
		protected override void OnDisable () {
			// this will no longer be called
			
			// If I am hooking into ActionManager
			
			if (hub != null)
				hub.onAction -= _OnAction;
				
			
			
		}
		
		
		
		protected override void _OnAction (ActionEventDetail data) {
			// none of the messages make sense unless I know who sent it, and who receives it
			
			if (mute) return;
			OnAction (data);
			
			
				
		}
		
		
	
	}
}