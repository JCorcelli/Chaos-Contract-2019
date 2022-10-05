using UnityEngine;
using System.Collections;

using ActionSystem.Subscribers;
using ActionSystem;

namespace Utility.Managers
{
	public abstract class AbstractTriggerHUBSubscriber : MonoBehaviour {
		/*
			does not globalize
			
			
		*/
		
		
		public AbstractTriggerHUB hub;
		protected virtual void Awake () {
			// during initialization and whenever it is enabled later, this will subscribe/resubscribe to either manager or current ActionHandler
			
			
			// If I am hooking into ActionManager
			if (hub == null)
				hub = gameObject.GetComponentInParent<AbstractTriggerHUB>();
			if (hub == null)
				Debug.Log(gameObject.name + ": Warn: no HUB manager or Group HUB",gameObject);
			
				
		}	
		protected virtual void OnEnable () {
			// during initialization and whenever it is enabled later, this will subscribe/resubscribe to manager
			
			
			// If I am hooking into HUB
			if (hub != null)
			{
				hub.onTriggerEnter += _OnEnter;
				hub.onTriggerExit += _OnExit;
			}
			
				
				
		}
		protected virtual void OnDisable () {
			// this will no longer be called
			
			// If I am hooking into ActionManager
			
			if (hub != null)
			{
				hub.onTriggerEnter -= _OnEnter;
				hub.onTriggerExit -= _OnExit;
			}
				
			
			
		}
		
		
		
		protected virtual void _OnEnter() {
			
			OnEnter ();
			
			
				
		}
		protected virtual void _OnExit() {
			
			OnExit ();
			
			
				
		}
		
		protected abstract void OnEnter();
		protected abstract void OnExit();
		
		
		
	
	}
}