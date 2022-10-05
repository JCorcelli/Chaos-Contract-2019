using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public abstract class AbstractKeyHandler : UpdateBehaviour
	{

		public string buttonName = "mouse 1";
		protected bool _activeInternal = false;
		
		
		protected override void OnEnable() {
			base.OnEnable();
			
			SelectionManager.onPress += _OnPress;
		}
		
			
		protected override void OnDisable() {
			base.OnDisable();
			SelectionManager.onPress -= _OnPress;
		}
		
		protected override void _OnPress(){
			if (Input.GetKeyDown( buttonName) ){
				_pressed = true;
				OnPress();
			}
				
		}
		protected virtual void OnPress(){}
		
		protected bool _pressed = false;
		
		
		protected override void OnUpdate() {
			if (_pressed) _OnHold();
		}
			
		protected virtual void _OnHold(){
			
			if (Input.GetKeyUp(  buttonName) )
			{
				_pressed = false;
				OnRelease();
			}
			
			else if (Input.GetKey(  buttonName) )
				OnHold();
				
		}
		protected virtual void OnHold(){}
		
		protected virtual void OnRelease(){}
	
	}
}