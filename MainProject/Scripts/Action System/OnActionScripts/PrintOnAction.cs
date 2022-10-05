using UnityEngine;
using System.Collections;

namespace ActionSystem.OnActionScripts {
	public class PrintOnAction : MonoBehaviour, IOnAction {

		// Use this for initialization
		public string whatAction = "";
		
		public void OnAction (ActionEventDetail data) {
			if (data.what.ToLower() == whatAction.ToLower())
			{
				Debug.Log(name,gameObject);
			}
		}
	}
}