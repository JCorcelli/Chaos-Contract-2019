using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionSystem.OnActionScripts {


	public class ChangeTextToHiding : MonoBehaviour, IOnAction {
		
		private Text t;
		
		void Awake() { 
		t = GetComponent<Text>(); 
		}
		public void OnAction(ActionEventDetail data) {
			if (data.what == "Entering")
				t.text = "Hiding";
			else if (data.what == "Leaving")
				t.text = "Caution";
			// if he's playing I guess it be callous
			
		}
	}
}