using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ActionSystem.OnActionScripts;
using ActionSystem;


namespace Utility.Managers
{
	
	public class ListenForAnyEffect : EffectHUBSubscriber {
		
		
		protected ActionDelegate onAction;
		
		protected override void Awake() { 
			base.Awake();
			if (GetComponent<IOnAction>() == null) {
				Debug.Log(gameObject.name + " Warn: you need IOnAction interface",gameObject);
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