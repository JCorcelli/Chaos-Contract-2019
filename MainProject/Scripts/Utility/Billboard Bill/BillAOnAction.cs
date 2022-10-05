using UnityEngine;
using System.Collections;
using ActionSystem.OnActionScripts;
using ActionSystem;

namespace Utility
{
	public class BillAOnAction : MonoBehaviour, IOnAction {
		public string whatAction = "";
		public string text = "";

		// Use this for initialization
		public BillAParent billParent;
		void Awake () {
		
			if (billParent == null) 
				billParent = GetComponent<BillAParent>();
			if (billParent == null) 
				Debug.Log(name + " has no bill parent",gameObject);
		}
		
		// Update is called once per frame
		public void  OnAction(ActionEventDetail data ) {
			if ( data.what == whatAction)
			{
				billParent.Add(text);
			}
		}
	}

}