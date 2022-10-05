using UnityEngine;
using System.Collections;

namespace ActionSystem.Subscribers
{
	public abstract class WhoSubscriber : AbstractSubscriber {
		/*
			used by all manner of things that
				
			Implementation methods:
				abstract void CallToAction(ActionEventData data)
			
			
		*/
		
		
	
		protected override void OnEnable () {
			// during initialization and whenever it is enabled later, this will subscribe/resubscribe to either manager or current ActionHandler
			
			
			// If I am hooking into ActionManager
			base.OnEnable();
			manager = ActionManager.instance;
			if (manager != null)
				manager.onRecord += _OnAction;
			else
				Debug.Log(gameObject.name + ": Warn: no action manager instance to subscribe",gameObject);
			
				
				
		}
		protected override void OnDisable () {
			// this will no longer be called
			
			// If I am hooking into ActionManager
			base.OnDisable();
			if (manager != null)
				manager.onRecord -= _OnAction;
				
			
			
		}
		
		
		
		public string actorName = "";
		
		protected override void _OnAction (ActionEventDetail data) {
			// none of the messages make sense unless I know who sent it, and who receives it
			
			if (this.actorName != data.who) return;
			base._OnAction (data);
			
			
				
		}
		
	
	}
}