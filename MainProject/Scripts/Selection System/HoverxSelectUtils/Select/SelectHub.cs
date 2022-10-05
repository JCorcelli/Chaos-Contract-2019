using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public interface ISelectHub
	{
		void OnEnter();
		void OnExit();
		void OnClick();
		void OnPress();
		void OnRelease();
		
		void OnSelect();
		void OnDeselect();
	}
	public class SelectHub : UpdateBehaviour, ISelectHub {
		
		
		// these may trigger multiple times for different nodes, but only one time.
		protected bool pressed = false;
		protected bool clicked = false;
		protected bool hovered = false;
		protected bool selected = false;
		
		protected override void OnDisable() {
			pressed = false;
			clicked = false;
			hovered = false;
			selected = false;

			
		}
		
		public virtual void  OnEnter() { hovered = true; }
		
		public virtual void  OnExit() { hovered = false; }
			
		public virtual void  OnClick() {
			// ensures a release occurred over this
			if (clicked) return;
			clicked = true; 
			
		}
		
		public virtual void  OnRelease() { 
			pressed = false; 
		}
		
		public virtual void  OnSelect() {
			selected = true;
		}
		
		public virtual void  OnDeselect() { 
			selected = false;
		}
		
		public virtual void  OnPress() { 
			pressed = true; 
		}
		
		protected override void OnLateUpdate(){
			clicked = false;
		}
	}
}