using UnityEngine;
using System.Collections;


namespace ActionSystem.OnActionScripts {
	public interface IOnAction  {

		// implement this
		void OnAction(ActionEventDetail data);
	}
}