using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	
		
	public delegate void HSCxControllerDelegate(HSCxController caller);
			
	public class HSCxController:  HoverxSelectAbstract {
		// This receives signals, and sends them only to subscribers
	
		public HSCxControllerDelegate onEnter;
		public HSCxControllerDelegate onExit;
		public HSCxControllerDelegate onPress;
		public HSCxControllerDelegate onRelease;
		public HSCxControllerDelegate onClick;
		public HSCxControllerDelegate onSelect;
		public HSCxControllerDelegate onDeselect;
		public HSCxControllerDelegate doWhilePressed;
		public HSCxControllerDelegate doWhileActive;
		public HSCxControllerDelegate doWhileHovered;
		// could do a last call
		
		protected void Call(HSCxControllerDelegate module){
			if (module != null) module(this);
		}
		
		public override void  OnEnter() {Call(onEnter); }
		
		public override void  OnExit() {Call(onExit); }
			
		public override  void  OnClick() {Call(onClick); }
		
		public override  void  OnPress() {Call(onPress); }
		public override  void  OnRelease() {Call(onRelease); }
		
		public override  void  OnSelect() {Call(onSelect); }
		
		public override  void  OnDeselect() {Call(onDeselect); }
		
		public override void  DoWhilePressed() {Call(doWhilePressed); }
		
		public override void  DoWhileActive() {Call(doWhileActive); }
		
		public override void  DoWhileHovered() {Call(doWhileHovered); }
		
	}
}