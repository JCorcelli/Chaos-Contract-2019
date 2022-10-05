using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public class OnPressDisableChild : AbstractButtonHandler {

		
		protected GameObject child;
		
		void Awake() { 
			child = transform.GetChild(0).gameObject; 
		}
		protected override void OnPress () {
			if (SelectGlobal.locked ) return;
			child.SetActive(false);
			
		}
		
	}
}