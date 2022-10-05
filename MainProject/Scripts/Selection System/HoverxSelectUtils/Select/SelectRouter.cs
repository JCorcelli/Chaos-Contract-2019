using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public delegate void SelectRouterDelegate();
	
	
	public class SelectRouter: UpdateBehaviour, ISelectHub {
		// this sends signals in many directions
		
		
		public bool used = false;
		
		
		public SelectRouterDelegate onEnter;
		public SelectRouterDelegate onExit;
		public SelectRouterDelegate onPress;
		public SelectRouterDelegate onRelease;
		public SelectRouterDelegate onClick;
		public SelectRouterDelegate onSelect;
		public SelectRouterDelegate onDeselect;
		public SelectRouterDelegate doWhilePressed;
		public SelectRouterDelegate doWhileActive;
		public SelectRouterDelegate doWhileHovered;
		
		// these may trigger multiple times for different nodes, but only one time.
		public bool pressed = false;
		public bool clicked = false;
		public bool hovered = false;
		public bool selected = false;
		
		protected override void OnDisable() {
			pressed = false;
			clicked = false;
			hovered = false;
			selected = false;

			
		}
		
		
		
		
		
		protected void Call(SelectRouterDelegate module){
			if (module != null) module();
			
		}
		
		public virtual void  OnEnter() { hovered = true; Call(onEnter); }
		
		public virtual void  OnExit() {hovered = false; Call(onExit); }
			
		public virtual  void  OnClick() {clicked = true; Call(onClick); 
		}
		
		public virtual  void  OnPress() {pressed = true; Call(onPress); }
		public virtual  void  OnRelease() {pressed = false; Call(onRelease); }
		
		public virtual  void  OnSelect() {selected = true; Call(onSelect); }
		
		public virtual  void  OnDeselect() {selected = false; Call(onDeselect); }
		
		protected virtual void  DoWhilePressed() {Call(doWhilePressed); }
		
		protected virtual void  DoWhileActive() {Call(doWhileActive); }
		
		protected virtual void  DoWhileHovered() {Call(doWhileHovered); }
		
		protected override void OnUpdate() {
			base.OnUpdate();
			if (pressed) DoWhilePressed();
			if (hovered) DoWhileHovered();
			if (selected) DoWhileActive();
		}
		
		protected override void OnLateUpdate() {
			
			clicked = false;
		}
		
	}
}