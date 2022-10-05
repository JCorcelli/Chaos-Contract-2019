using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace ActionSystem.OnActionScripts
{
	public abstract class OnLocationAbstract :  MonoBehaviour, IOnAction {
		
		
		public void  OnAction(ActionEventDetail data ) {
			if ( data.where != location) return;
			if ( data.what == enterAction )
				OnEnter(data); // this pose
			else if (data.what == exitAction )
				OnExit(data); // or whatever number i need to reset the pose
				
		}
		
		public string enterAction = "Enter";
		public string exitAction = "Exit";
		public string location = "Cage";
		
		protected abstract void OnEnter(ActionEventDetail data);
		protected abstract void OnExit(ActionEventDetail data);
		
	}
}