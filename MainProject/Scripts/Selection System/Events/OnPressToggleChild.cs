using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public class OnPressToggleChild : AbstractButtonHandler {

		
		protected GameObject child;
		
		void Awake() { 
			child = transform.GetChild(0).gameObject; 
		}
		protected override void OnPress () {
			if (!gameObject.activeInHierarchy || SelectGlobal.locked ) return;
			child.SetActive(!child.activeSelf);
		}
		
	}
}