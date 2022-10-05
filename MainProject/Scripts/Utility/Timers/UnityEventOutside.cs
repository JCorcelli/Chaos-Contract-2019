using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using ActionSystem.OnActionScripts;
using ActionSystem;

namespace Utility.Timers
{
	
	public class UnityEventOutside : OnLocationAbstract {

		[System.Serializable]
		public class ReactionEvent : UnityEvent { }
		public ReactionEvent ExitEvent;
		public ReactionEvent EnterEvent;
		
		
		
		protected override void OnEnter(ActionEventDetail data) {
			if (EnterEvent != null) EnterEvent.Invoke();
			}
		protected override void OnExit(ActionEventDetail data){
			if (ExitEvent != null) ExitEvent.Invoke();
			
			}
		
	}
}