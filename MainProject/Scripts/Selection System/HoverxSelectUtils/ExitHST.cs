using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public class ExitHST:  HoverxSelectToggle {

		public GameObject onExit;
		
		
		public override void  OnEnter() {
			
			onExit.SetActive(false);
		}
		
		public override void  OnExit() {
			if (isActive) return;
			onExit.SetActive(true);
		}
		
		
		
	}
}