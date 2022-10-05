using UnityEngine;
using System.Collections;
using ActionSystem.OnActionScripts;
using ActionSystem;

namespace Utility.Timers
{
	
	public class ToggleOtherOutside : OnLocationAbstract {

		// Use this for initialization
		public GameObject other;
		protected void Awake(){
			if (other == null)
				other = transform.GetChild(0).gameObject;
			if (other == null) 
			{
				Debug.Log(name + " does not have an object to toggle",gameObject);
				enabled = false;
			}
		}
		protected override void OnEnter(ActionEventDetail data){other.SetActive(false);}
		protected override void OnExit(ActionEventDetail data){other.SetActive(true);}
		
	}
}