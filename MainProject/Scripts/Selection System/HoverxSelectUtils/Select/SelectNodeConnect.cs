using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public class SelectNodeConnect : SelectAbstract {
		// this sends a signal to a single hub
		
		public ISelectHub controller {set{ ih = value; } get{ return ih;}
		}
		
		protected ISelectHub ih;
		
		protected override void OnEnable(){
			base.OnEnable();
			Connect();
		}
		protected void Connect() {
				
			ih = gameObject.GetComponentInParent<ISelectHub>();
			if (ih == null) Debug.LogError(name + " has no SelectHub", gameObject);
			
		}
		
		// these may trigger multiple times for different nodes, but only one time.
		protected override void  OnEnter() { ih.OnEnter(); }
		
		protected override void  OnExit() { ih.OnExit(); }
			
		protected override  void  OnClick() { ih.OnClick(); }
		
		protected override  void  OnPress() { ih.OnPress(); }
		protected override  void  OnRelease() { ih.OnRelease(); }
		
		protected override  void  OnSelect() { ih.OnSelect(); }
		
		protected override  void  OnDeselect() {ih.OnDeselect(); }
		
		
	}
}