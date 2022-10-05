using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace ActionSystem.OnActionScripts
{
	public abstract class OnRegisterAbstract :  MonoBehaviour, IOnAction {
		
		
		public void  OnAction(ActionEventDetail data ) {
			if ( data.what == beginAction )
				OnBegin(data); // this pose
			else if (data.what == finishAction )
				OnFinish(data); // or whatever number i need to reset the pose
				
		}
		
		public string beginAction = "Enter";
		public string finishAction = "Exit";
		
		protected abstract void OnBegin(ActionEventDetail data);
		protected abstract void OnFinish(ActionEventDetail data);
		
	}
}