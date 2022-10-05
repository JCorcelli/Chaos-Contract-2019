using UnityEngine;
using System.Collections;


namespace ActionSystem.OnActionScripts
{
	public class EnableOtherOnAction: MonoBehaviour, IOnAction {

	
		public GameObject other;
		public string enableAction = "enable";
		public string disableAction = "disable";
		public bool onStart = false;
		
		void Awake() {
			other.SetActive(onStart);
		}
		public void OnAction(ActionEventDetail data) {
			if (data.what.ToLower() == enableAction.ToLower())
				other.SetActive(true);
			else 
			if (data.what.ToLower() == disableAction.ToLower())
				other.SetActive(false);
				
		}
	}
}