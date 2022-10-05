using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ActionSystem.OnActionScripts;


namespace ActionSystem.Subscribers
{
	
	public class ListenForAnyAction : OnActionSubscriber {
		
		
		protected ActionDelegate onAction;
		
		void Awake() { 
			if (GetComponent<IOnAction>() == null) {
				Debug.Log("you need IOnAction interface",gameObject);
				enabled = false;
			}
			
			foreach (IOnAction a in GetComponents<IOnAction>())
				onAction += a.OnAction;
		}
		
		protected override void OnAction(ActionEventDetail data) {
			onAction(data);
		}
	}
}