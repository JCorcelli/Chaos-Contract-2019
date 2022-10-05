using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionSystem.OnActionScripts {


	public class ChangeTextToWhat : MonoBehaviour, IOnAction {
		
		private Text t;
		
		void Awake() { 
		t = GetComponent<Text>(); 
		}
		public void OnAction(ActionEventDetail data) {
			t.text = data.what;
		}
	}
}