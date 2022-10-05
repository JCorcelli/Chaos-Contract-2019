using UnityEngine;
using System.Collections;

namespace ActionSystem.OnActionScripts {
	public class EnableChildOnAction : MonoBehaviour, IOnAction {

		// Use this for initialization
		public string whatAction = "";
		
		private Transform child;
		void Awake () { child = transform.GetChild(0);  }
		void Start () { child.gameObject.SetActive( false); }
		public void OnAction (ActionEventDetail data) {
			if (data.what.ToLower() == whatAction.ToLower())
			{
				child.gameObject.SetActive( true );
			}
		}
	}
}