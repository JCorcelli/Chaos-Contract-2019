using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public abstract class SelectNodeAbstract : SelectAbstract {

		
		protected override void  OnEnter() {  }
		
		protected override void  OnExit() { }
			
		protected override  void  OnClick() {  }
		
		protected override  void  OnPress() { }
		protected override  void  OnRelease() { }
		
		protected override  void  OnSelect() {  }
		
		protected override  void  OnDeselect() { }
		
		
	}
}