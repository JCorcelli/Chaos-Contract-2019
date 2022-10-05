using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public delegate void SelectRelayDelegate();
	
	
	public class SelectRelay: SelectAbstract {
		// this sends signals in many directions
		
		public SelectRelay controller {set{ ih = value; } get{ return ih;}
		}
		public bool used = false;
		protected SelectRelay ih;
		public SelectRelay caller;
		
		public SelectRelayDelegate onEnter;
		public SelectRelayDelegate onExit;
		public SelectRelayDelegate onPress;
		public SelectRelayDelegate onRelease;
		public SelectRelayDelegate onClick;
		public SelectRelayDelegate onSelect;
		public SelectRelayDelegate onDeselect;
		public SelectRelayDelegate doWhilePressed;
		public SelectRelayDelegate doWhileActive;
		public SelectRelayDelegate doWhileHovered;
		
		
		protected virtual void Start() {
			
			if (ih == null) Connect();
		}
		
		
		protected virtual void OnDestroy(){
			
			if (ih == null) return;
			onEnter 		-= ih.onEnter;
			onExit 			-= ih.onExit;
			onPress 		-= ih.onPress;
			onRelease 		-= ih.onRelease;
			onClick 		-= ih.onClick;
			onSelect 		-= ih.onSelect;
			onDeselect 		-= ih.onDeselect;
			
		}
		
		
		protected override void OnEnable(){
			base.OnEnable();
			
			if (ih == null) 
			{
				Connect();
				
			}
		}
		protected void Connect() {
				
			ih = transform.parent.GetComponentInParent<SelectRelay>();
			
			if (ih == null) return;
			
			onEnter 		+= ih.onEnter;
			onExit 			+= ih.onExit;
			onPress 		+= ih.onPress;
			onRelease 		+= ih.onRelease;
			onClick 		+= ih.onClick;
			onSelect 		+= ih.onSelect;
			onDeselect 		+= ih.onDeselect;
			
		}
		
		
		protected void Call(SelectRelayDelegate module){
			if (module != null) module();
			
		}
		
		protected override void  OnEnter() {Call(onEnter); }
		
		protected override void  OnExit() {Call(onExit); }
			
		protected override  void  OnClick() {Call(onClick); 
		}
		
		protected override  void  OnPress() {Call(onPress); }
		protected override  void  OnRelease() {Call(onRelease); }
		
		protected override  void  OnSelect() {Call(onSelect); }
		
		protected override  void  OnDeselect() {Call(onDeselect); }
		
		
		
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			used = false;
		}
		
	}
}