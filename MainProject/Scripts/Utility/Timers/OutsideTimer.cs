using UnityEngine;
using System.Collections;
using ActionSystem.OnActionScripts;
using ActionSystem;

namespace Utility.Timers
{
	
	public class OutsideTimer : OnLocationAbstract {

		// Use this for initialization
		protected EggTimerText timer;
		protected void Awake(){
			timer = GetComponent<EggTimerText>();
			if (timer == null) Debug.Log(name + " does not have EggTimerText",gameObject);
		}
		protected override void OnEnter(ActionEventDetail data){timer.enabled = false;}
		protected override void OnExit(ActionEventDetail data){timer.enabled = true;}
		
	}
}