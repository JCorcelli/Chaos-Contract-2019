using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public class SelectRelayAddAbstract : SelectButtonComboPrecision {
		// this sends a signal to a single hub
		
		public SelectRelay controller {set{ ih = value; } get{ return ih;}
		}
		
		protected SelectRelay ih;
		
		
		protected override void OnEnable(){
			base.OnEnable();
			Connect();
			
			ih.onEnter 		+= OnEnter;
			ih.onExit 		+= OnExit;
			ih.onPress 		+= OnPress;
			ih.onRelease 	+= OnRelease;
			ih.onClick 		+= OnClick;
			ih.onSelect 	+= OnSelect;
			ih.onDeselect 	+= OnDeselect;
			
		}
		protected override void OnDisable(){
			base.OnDisable();
			
			ih.onEnter 		-= OnEnter;
			ih.onExit 		-= OnExit;
			ih.onPress 		-= OnPress;
			ih.onRelease 	-= OnRelease;
			ih.onClick 		-= OnClick;
			ih.onSelect 	-= OnSelect;
			ih.onDeselect 	-= OnDeselect;
			
		}
		protected void Connect() {
			
			ih = gameObject.GetComponentInParent<SelectRelay>();
			if (ih == null) Debug.LogError(name + " has no SelectRelay", gameObject);
			
		}
		
		
		protected virtual void  OnEnter() {}
		protected virtual void  OnExit() {}
		protected virtual void  OnClick() {}
		protected override void  OnPress() {}
		protected override void  OnRelease() {}
		protected virtual void  OnSelect() {}
		protected virtual void  OnDeselect() {}
		
		
		
	}
}