using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public class OnReleaseToggleChild : AbstractButtonHandler {

		
		protected GameObject child;
		
		void Awake() { 
			child = transform.GetChild(0).gameObject; 
		}
		protected override void OnRelease () {
			if (SelectGlobal.locked ) return;
			child.SetActive(!child.activeSelf);
		}
		
	}
}