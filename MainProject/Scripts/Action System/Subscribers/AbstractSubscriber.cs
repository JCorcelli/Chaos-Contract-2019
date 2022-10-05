using UnityEngine;
using System.Collections;

namespace ActionSystem.Subscribers
{
	public abstract class AbstractSubscriber : UpdateBehaviour {
		/*
		
			
		Implementation methods:
			abstract void OnAction(ActionEventData data)
			
			virtual void OnEnable ()
			virtual void OnDisable ()  
			
			
			
			
		
		*/
		
		
		protected ActionManager manager;
		protected bool submittedThisFrame = false;
		public bool mute = false; // easy way to avoid repeating
		
		
		
		
		
		protected bool warned = false;
		protected virtual void _OnAction (ActionEventDetail data) {
			if (mute) return;
			
			if 
			(submittedThisFrame) 
			{
				if (!warned) 
				{ Debug.Log("Warn, a delay of one frame is required Action:" + data.what + " was not submitted properly",gameObject); 
				warned = true;
				}
				return;
			}
			
			submittedThisFrame = true; // prevent recursion from happening during the call to action

			OnAction (data);
			
			submittedThisFrame = false;
				
		}
		
		
		
		abstract protected void OnAction (ActionEventDetail data) ;
			/*read from action
				who: 		
				doing what: 
				when: 		
				where: 		
				why: 		
				how?: 		
				*/
		
		
		protected virtual void Submit(ActionEventDetail data){
				ActionManager.Submit(data);
		}		
		
					
	}
}