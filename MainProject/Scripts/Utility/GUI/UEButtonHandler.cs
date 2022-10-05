using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;// Required when using Event data.
using SelectionSystem;

namespace Utility.GUI
{
	public class UEButtonHandler : EventTrigger
	{
		
		public string buttonName = "mouse 1";
		
		public override void OnPointerClick(PointerEventData data){
			if (!Input.GetButtonUp(buttonName)) return;
			base.OnPointerClick(data);
		}
	}
}