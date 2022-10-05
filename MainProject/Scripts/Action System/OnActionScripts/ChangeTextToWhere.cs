using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionSystem.OnActionScripts {


	public class ChangeTextToWhere : MonoBehaviour, IOnAction {
		
		private Text t;
		public string defaultText = "Room";
		
		void Awake() { 
			t = GetComponent<Text>(); 
			if (t == null) Debug.Log(gameObject.name + " Warn: needs text",gameObject);
		}
		public void OnAction(ActionEventDetail data) {
			if (data.where != "")
				t.text = data.where;
		}
	}
}