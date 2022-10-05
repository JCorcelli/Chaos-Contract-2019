using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionSystem.OnActionScripts {


	public class ChangeTextToVisibility : MonoBehaviour, IOnAction {
		
		private Text t;
		
		void Awake() { 
			t = GetComponent<Text>(); 
		}
		public void OnAction(ActionEventDetail data) {
			if (data.what == "SetVisible")
			{
					t.text = data.how;
				
			}
		}
	}
}